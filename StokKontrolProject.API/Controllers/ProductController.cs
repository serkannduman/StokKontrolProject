using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StokKontrolProject.Domain.Entities;
using StokKontrolProject.Service.Abstract;

namespace StokKontrolProject.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IGenericService<Product> _service;

        public ProductController(IGenericService<Product> service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult TumUrunleriGetir()
        {
           
            return Ok(_service.GetAll(t0=>t0.Category,t1=>t1.Supplier));
        }

        [HttpGet]
        public IActionResult AktifUrunleriGetir()
        {
            return Ok(_service.GetActive(t0 => t0.Category, t1 => t1.Supplier));
        }

        [HttpGet("{id}")]
        public IActionResult IdyeGoreUrunGetir(int id)
        {
            return Ok(_service.GetById(id));
        }

        [HttpPost]
        public IActionResult UrunEkle(Product product)
        {
            _service.Add(product);
            //return Ok("Basarılı"); //Boyle de yapabilirdik
            return CreatedAtAction("IdyeGoreUrunGetir", new { id = product.ID }, product);
        }

        [HttpPut("{id}")]
        public IActionResult UrunGuncelle(int id, Product product)
        {
            if (id != product.ID)
                return BadRequest();

            try
            {
                _service.Update(product);
                return Ok(product);
            }
            catch (DbUpdateConcurrencyException)
            {

                if (!UrunVarmi(id))
                    return NotFound();
            }

            return NoContent();
        }

        private bool UrunVarmi(int id)
        {
            return _service.Any(cat => cat.ID == id); //Eğer parametrede gelen id'ye göre kategori var ise true yoksa false dönecektir.
        }

        [HttpDelete("{id}")]
        public IActionResult UrunSil(int id)
        {
            var product = _service.GetById(id);

            if (product == null)
                return NotFound();

            try
            {
                _service.Remove(product);
                return Ok("Kategori Silindi");
            }
            catch (Exception)
            {

                return BadRequest();
            }
        }

        [HttpGet("{id}")]
        public IActionResult UrunAktiflestir(int id)
        {
            var product = _service.GetById(id);

            if (product == null)
                return NotFound();

            try
            {
                _service.Activate(id);
                return Ok(_service.GetById(id));
            }
            catch (Exception)
            {

                return BadRequest();
            }
        }
    }
}
