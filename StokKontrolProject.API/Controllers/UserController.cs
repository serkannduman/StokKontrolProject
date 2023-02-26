using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StokKontrolProject.Domain.Entities;
using StokKontrolProject.Service.Abstract;

namespace StokKontrolProject.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IGenericService<User> _service;

        public UserController(IGenericService<User> service)
        {
            _service = service;
        }
        [HttpGet]
        public IActionResult TumKullanicilariGetir()
        {
            return Ok(_service.GetAll()); //Ok --> 200 başarılı durum kodunu döndürüyor
        }

        [HttpGet]
        public IActionResult AktifKullanicilariGetir()
        {
            return Ok(_service.GetActive());
        }

        [HttpGet("{id}")]
        public IActionResult IdyeGoreKullaniciGetir(int id)
        {
            return Ok(_service.GetById(id));
        }
        [HttpPost]
        public IActionResult KullaniciEkle(User user)
        {
            _service.Add(user);
            //return Ok("Basarılı"); //Boyle de yapabilirdik
            return CreatedAtAction("IdyeGoreKullaniciGetir", new { id = user.ID }, user);
        }
        [HttpPut("{id}")]
        public IActionResult KullaniciGuncelle(int id, User user)
        {
            if (id != user.ID)
                return BadRequest();

            try
            {
                _service.Update(user);
                return Ok(user);
            }
            catch (DbUpdateConcurrencyException)
            {

                if (!KullaniciVarmi(id))
                    return NotFound();
            }

            return NoContent();
        }

        private bool KullaniciVarmi(int id)
        {
            return _service.Any(cat => cat.ID == id); //Eğer parametrede gelen id'ye göre kategori var ise true yoksa false dönecektir.
        }

        [HttpDelete("{id}")]
        public IActionResult KullaniciSil(int id)
        {
            var user = _service.GetById(id);

            if (user == null)
                return NotFound();

            try
            {
                _service.Remove(user);
                return Ok("Kullanici Silindi");
            }
            catch (Exception)
            {

                return BadRequest();
            }
        }

        [HttpGet("{id}")]
        public IActionResult KullaniciAktiflestir(int id)
        {
            var user = _service.GetById(id);

            if (user == null)
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

        [HttpGet]
        public IActionResult Login(string email, string password)
        {
            if(_service.Any(user => user.Email==email && user.Password == password))
            {
                User loggedUser = _service.GetByDefault(user => user.Email == email && user.Password == password);
                return Ok(loggedUser);
            }

            return NotFound();
        }
    }
}
