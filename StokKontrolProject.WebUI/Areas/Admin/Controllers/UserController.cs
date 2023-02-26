using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StokKontrolProject.Domain.Entities;
using StokKontrolProject.WebUI.Models;
using System.Text;

namespace StokKontrolProject.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly IWebHostEnvironment _environment;
        public UserController(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        string uri = "https://localhost:7235";
        public async Task<IActionResult> Index()
        {
            //Tüm kategorileri listeleyeceğiz.

            List<User> kullanicilar = new List<User>();

            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{uri}/api/User/TumKullanicilariGetir"))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();

                    kullanicilar = JsonConvert.DeserializeObject<List<User>>(apiCevap);
                }
            }

            return View(kullanicilar);
        }

        [HttpGet]
        public async Task<IActionResult> ActivateUser(int id)
        {

            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{uri}/api/User/KullaniciAktiflestir/{id}"))
                {

                }
            }

            return RedirectToAction("Index");
        }


        public async Task<IActionResult> DeleteUser(int id)
        {

            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.DeleteAsync($"{uri}/api/User/KullaniciSil/{id}"))
                {

                }
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult AddUser()
        {
            return View(); //sadece ekleme View'ını gösterecek.
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(User user,List<IFormFile> files)
        {
            user.IsActive = true;
            user.Password = "123456a.";
            string returnedMessage = Upload.ImageUpload(files, _environment, out bool imgResult);

            if (imgResult)
            {
                user.PhotoURL = returnedMessage;
            }
            else
            {
                ViewBag.Message = returnedMessage;
                return View();
            }

            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json"); //Türkçe karakterini destekleyen json oluşturduk.

                using (var cevap = await httpClient.PostAsync($"{uri}/api/User/KullaniciEkle", content))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();


                }
            }

            return RedirectToAction("Index");
        }

        static User updatedUser;
        [HttpGet]
        public async Task<IActionResult> UpdateUser(int id) //id ile iligili kategoriyi bul ve viewde göster
        {
            

            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{uri}/api/User/IdyeGoreKullaniciGetir/{id}"))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();

                    updatedUser = JsonConvert.DeserializeObject<User>(apiCevap);
                }
            }

            return View(updatedUser);


            
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUser(User guncelKullanici, List<IFormFile> files)
        {
            if (files.Count == 0) //Güncelleme esnasında foto yüklenmez ise
            {
                guncelKullanici.PhotoURL = updatedUser.PhotoURL;
            }
            else
            {
                string returnedMessage = Upload.ImageUpload(files, _environment, out bool imgResult);

                if (imgResult)
                {
                    guncelKullanici.PhotoURL = returnedMessage;
                }
                else
                {
                    ViewBag.Message = returnedMessage;
                    return View();
                }
            }

            using (var httpClient = new HttpClient())
            {
                guncelKullanici.AddedDate = updatedUser.AddedDate;
                guncelKullanici.IsActive = updatedUser.IsActive;
                guncelKullanici.Password = updatedUser.Password;
                StringContent content = new StringContent(JsonConvert.SerializeObject(guncelKullanici), Encoding.UTF8, "application/json"); //Türkçe karakterini destekleyen json oluşturduk.

                using (var cevap = await httpClient.PutAsync($"{uri}/api/User/KullaniciGuncelle/{guncelKullanici.ID}", content))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();


                }
            }

            return RedirectToAction("Index");
        }
    }
}
