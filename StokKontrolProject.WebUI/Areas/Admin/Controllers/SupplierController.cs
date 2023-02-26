using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StokKontrolProject.Domain.Entities;
using System.Text;

namespace StokKontrolProject.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SupplierController : Controller
    {
        string uri = "https://localhost:7235";
        public async Task<IActionResult> Index()
        {
            //Tüm kategorileri listeleyeceğiz.

            List<Supplier> tedarikciler = new List<Supplier>();

            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{uri}/api/Supplier/TumTedarikcileriGetir"))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();

                    tedarikciler = JsonConvert.DeserializeObject<List<Supplier>>(apiCevap);
                }
            }

            return View(tedarikciler);
        }

        [HttpGet]
        public async Task<IActionResult> ActivateSupplier(int id)
        {

            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{uri}/api/Supplier/TedarikciAktiflestir/{id}"))
                {

                }
            }

            return RedirectToAction("Index");
        }


        public async Task<IActionResult> DeleteSupplier(int id)
        {

            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.DeleteAsync($"{uri}/api/Supplier/TedarikciSil/{id}"))
                {

                }
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult AddSupplier()
        {
            return View(); //sadece ekleme View'ını gösterecek.
        }

        [HttpPost]
        public async Task<IActionResult> AddSupplier(Supplier supplier)
        {
            supplier.IsActive = true;

            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(supplier), Encoding.UTF8, "application/json"); //Türkçe karakterini destekleyen json oluşturduk.

                using (var cevap = await httpClient.PostAsync($"{uri}/api/Supplier/TedarikciEkle", content))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();


                }
            }

            return RedirectToAction("Index");
        }

        static Supplier updatedSupplier;
        [HttpGet]
        public async Task<IActionResult> UpdateSupplier(int id) //id ile iligili kategoriyi bul ve viewde göster
        {
            List<Supplier> tedarikciler = new List<Supplier>();

            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{uri}/api/Supplier/IdyeGoreTedarikciGetir/{id}"))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();

                    updatedSupplier = JsonConvert.DeserializeObject<Supplier>(apiCevap);
                }
            }

            return View(updatedSupplier);


            return View(); //İlgili nesne ile güncelleme View'ını gösterecek.
        }

        [HttpPost]
        public async Task<IActionResult> UpdateSupplier(Supplier guncelTedarikci)
        {


            using (var httpClient = new HttpClient())
            {
                guncelTedarikci.AddedDate = updatedSupplier.AddedDate;
                guncelTedarikci.IsActive = updatedSupplier.IsActive;
                StringContent content = new StringContent(JsonConvert.SerializeObject(guncelTedarikci), Encoding.UTF8, "application/json"); //Türkçe karakterini destekleyen json oluşturduk.

                using (var cevap = await httpClient.PutAsync($"{uri}/api/Supplier/TedarikciGuncelle/{guncelTedarikci.ID}", content))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();


                }
            }

            return RedirectToAction("Index");
        }
    }
}
