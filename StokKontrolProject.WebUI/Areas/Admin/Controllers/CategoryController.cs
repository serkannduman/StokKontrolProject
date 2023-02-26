using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StokKontrolProject.Domain.Entities;
using System.Collections.Generic;
using System.Text;

namespace StokKontrolProject.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        string uri = "https://localhost:7235";
        public async Task<IActionResult> Index()
        {
            //Tüm kategorileri listeleyeceğiz.

            List<Category> kategoriler = new List<Category>();

            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{uri}/api/Category/TumKategorileriGetir"))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();

                    kategoriler = JsonConvert.DeserializeObject<List<Category>>(apiCevap);
                }
            }

            return View(kategoriler);
        }

        [HttpGet]
        public async Task<IActionResult> ActivateCategory(int id)
        {

            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{uri}/api/Category/KategoriAktiflestir/{id}"))
                {

                }
            }

            return RedirectToAction("Index");
        }


        public async Task<IActionResult> DeleteCategory(int id)
        {

            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.DeleteAsync($"{uri}/api/Category/KategoriSil/{id}"))
                {

                }
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult AddCategory()
        {
            return View(); //sadece ekleme View'ını gösterecek.
        }

        [HttpPost]
        public async Task<IActionResult> AddCategory(Category category)
        {
            category.IsActive = true;

            using(var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(category), Encoding.UTF8,"application/json"); //Türkçe karakterini destekleyen json oluşturduk.

                using (var cevap = await httpClient.PostAsync($"{uri}/api/Category/KategoriEkle", content))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();

                    
                }
            }

            return RedirectToAction("Index");
        }

        static Category updatedCategory;
        [HttpGet]
        public async Task<IActionResult> UpdateCategory(int id) //id ile iligili kategoriyi bul ve viewde göster
        {
            List<Category> kategoriler = new List<Category>();

            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{uri}/api/Category/IdyeGoreKategoriGetir/{id}"))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();

                    updatedCategory = JsonConvert.DeserializeObject<Category>(apiCevap);
                }
            }

            return View(updatedCategory);


            return View(); //İlgili nesne ile güncelleme View'ını gösterecek.
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCategory(Category guncelKategori)
        {
            

            using (var httpClient = new HttpClient())
            {
                guncelKategori.AddedDate = updatedCategory.AddedDate;
                guncelKategori.IsActive = updatedCategory.IsActive;
                StringContent content = new StringContent(JsonConvert.SerializeObject(guncelKategori), Encoding.UTF8, "application/json"); //Türkçe karakterini destekleyen json oluşturduk.

                using (var cevap = await httpClient.PutAsync($"{uri}/api/Category/KategoriGuncelle/{guncelKategori.ID}", content))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();


                }
            }

            return RedirectToAction("Index");
        }
    }
}
