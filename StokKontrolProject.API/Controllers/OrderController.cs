using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StokKontrolProject.Domain.Entities;
using StokKontrolProject.Service.Abstract;
using StokKontrolProject.Domain.Enums;

namespace StokKontrolProject.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IGenericService<User> _userService;
        private readonly IGenericService<Product> _productService;
        private readonly IGenericService<Order> _orderService;
        private readonly IGenericService<OrderDetail> _orderDetailService;

        public OrderController(IGenericService<User> userService, IGenericService<Product> productService, IGenericService<Order> orderService, IGenericService<OrderDetail> orderDetailService)
        {
            _userService = userService;
            _productService = productService;
            _orderService = orderService;
            _orderDetailService = orderDetailService;
        }

        [HttpGet]
        public IActionResult TumSiparisleriGetir()
        {
            return Ok(_orderService.GetAll(t0 => t0.OrderDetails, t1 => t1.User)); //Ok --> 200 başarılı durum kodunu döndürüyor
        }

        [HttpGet]
        public IActionResult AktifSiparisleriGetir()
        {
            return Ok(_orderService.GetActive(t0 => t0.OrderDetails, t1 => t1.User));
        }

        [HttpGet("{id}")]
        public IActionResult IdyeGoreSiparisGetir(int id)
        {
            return Ok(_orderService.GetByID(id, t0 => t0.OrderDetails, t1 => t1.User));
        }

        [HttpGet]
        public IActionResult BekleyenSiparisleriGetir()
        {
            return Ok(_orderService.GetDefault(x => x.Status == Status.Pending));
        }

        [HttpGet]
        public IActionResult OnaylananSiparisleriGetir()
        {
            return Ok(_orderService.GetDefault(x => x.Status == Status.Confirmed));
        }

        [HttpGet]
        public IActionResult ReddedilenSiparisleriGetir()
        {
            return Ok(_orderService.GetDefault(x => x.Status == Status.Cancelled));
        }

        [HttpPost]
        public IActionResult SiparisEkle(int userID, [FromQuery] int[] productIDs, [FromQuery] short[] quantities)
        {
            Order yeniSiparis = new Order();
            yeniSiparis.UserID = userID;
            yeniSiparis.Status = Status.Pending; //Eklenen sipariş bekliyor durumunda eklenecek
            yeniSiparis.IsActive = true; //Sipariş onaylanır ya da reddedilirse false'a cekilecek.

            _orderService.Add(yeniSiparis); //DB'ye eklendiğinde yeni bir order satırı ekleniyor ve ID oluşuyor.

            for (int i = 0; i < productIDs.Length; i++)
            {
                OrderDetail yeniDetay = new OrderDetail();
                yeniDetay.OrderID = yeniSiparis.ID;
                yeniDetay.ProductID = productIDs[i];
                yeniDetay.Quantity = quantities[i];
                yeniDetay.UnitPrice = _productService.GetById(productIDs[i]).UnitPrice;
                yeniDetay.IsActive = true;

                _orderDetailService.Add(yeniDetay);
            }

            //return Ok(yeniSiparis);
            return CreatedAtAction("IdyeGoreSiparisGetir", new { id = yeniSiparis.ID }, yeniSiparis);
        }

        [HttpGet("{id}")]
        public IActionResult SiparisOnayla(int id)
        {
            Order onaylananSiparis = _orderService.GetById(id);
            if (onaylananSiparis == null)
            {
                return NotFound();
            }
            else
            {
                List<OrderDetail> detaylar = _orderDetailService.GetDefault(x => x.OrderID == onaylananSiparis.ID).ToList();

                foreach (OrderDetail item in detaylar)
                {
                    Product siparistekiUrun = _productService.GetById(item.ProductID);
                    siparistekiUrun.Stock -= item.Quantity;
                    _productService.Update(siparistekiUrun);
                    item.IsActive = false;
                    _orderDetailService.Update(item);
                }

                onaylananSiparis.Status = Status.Confirmed;
                onaylananSiparis.IsActive = false;
                _orderService.Update(onaylananSiparis);

                return Ok(onaylananSiparis);
            }
        }

        [HttpGet("{id}")]
        public IActionResult SiparisReddet(int id)
        {
            Order reddedilenSiparis = _orderService.GetById(id);
            if (reddedilenSiparis == null)
            {
                return NotFound();
            }
            else
            {
                List<OrderDetail> detaylar = _orderDetailService.GetDefault(x => x.OrderID == reddedilenSiparis.ID).ToList();

                foreach (OrderDetail item in detaylar)
                {
                    item.IsActive = false;
                    _orderDetailService.Update(item);
                }

                reddedilenSiparis.Status = Status.Cancelled;
                reddedilenSiparis.IsActive = false;
                _orderService.Update(reddedilenSiparis);

                return Ok(reddedilenSiparis);

            }
        }


    }
}
