using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StokKontrolProject.Domain.Entities;
using StokKontrolProject.Service.Abstract;

namespace StokKontrolProject.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SupplierController : ControllerBase
    {
        private readonly IGenericService<Supplier> _service;

        public SupplierController(IGenericService<Supplier> service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult TumTedarikcileriGetir()
        {
            return Ok(_service.GetAll()); //Ok --> 200 başarılı durum kodunu döndürüyor
        }

        [HttpGet]
        public IActionResult AktifTedarikcileriGetir()
        {
            return Ok(_service.GetActive());
        }

        [HttpGet("{id}")]
        public IActionResult IdyeGoreTedarikciGetir(int id)
        {
            return Ok(_service.GetById(id));
        }
        [HttpPost]
        public IActionResult TedarikciEkle(Supplier supplier)
        {
            _service.Add(supplier);
            //return Ok("Basarılı"); //Boyle de yapabilirdik
            return CreatedAtAction("IdyeGoreTedarikciGetir", new { id = supplier.ID }, supplier);
        }
        [HttpPut("{id}")]
        public IActionResult TedarikciGuncelle(int id, Supplier supplier)
        {
            if (id != supplier.ID)
                return BadRequest();

            try
            {
                _service.Update(supplier);
                return Ok(supplier);
            }
            catch (DbUpdateConcurrencyException)
            {

                if (!TedarikciVarmi(id))
                    return NotFound();
            }

            return NoContent();
        }

        private bool TedarikciVarmi(int id)
        {
            return _service.Any(cat => cat.ID == id); //Eğer parametrede gelen id'ye göre kategori var ise true yoksa false dönecektir.
        }

        [HttpDelete("{id}")]
        public IActionResult TedarikciSil(int id)
        {
            var supplier = _service.GetById(id);

            if (supplier == null)
                return NotFound();

            try
            {
                _service.Remove(supplier);
                return Ok("Tedarikci Silindi");
            }
            catch (Exception)
            {

                return BadRequest();
            }
        }

        [HttpGet("{id}")]
        public IActionResult TedarikciAktiflestir(int id)
        {
            var supplier = _service.GetById(id);

            if (supplier == null)
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
