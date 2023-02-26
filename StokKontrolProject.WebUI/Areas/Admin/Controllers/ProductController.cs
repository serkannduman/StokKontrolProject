using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using StokKontrolProject.Domain.Entities;
using System.Text;

namespace StokKontrolProject.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        string uri = "https://localhost:7235";
        public async Task<IActionResult> Index()
        {
            //Tüm kategorileri listeleyeceğiz.

            List<Product> urunler = new List<Product>();

            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{uri}/api/Product/TumUrunleriGetir"))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();

                    urunler = JsonConvert.DeserializeObject<List<Product>>(apiCevap);
                }
            }

            return View(urunler);
        }

        [HttpGet]
        public async Task<IActionResult> ActivateProduct(int id)
        {

            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{uri}/api/Product/UrunAktiflestir/{id}"))
                {

                }
            }

            return RedirectToAction("Index");
        }


        public async Task<IActionResult> DeleteProduct(int id)
        {

            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.DeleteAsync($"{uri}/api/Product/UrunSil/{id}"))
                {

                }
            }

            return RedirectToAction("Index");
        }

        static List<Category> aktifKategoriler;
        static List<Supplier> aktifTedarikciler;

        [HttpGet]
        public async Task<IActionResult> AddProduct()
        {
            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{uri}/api/Category/AktifKategorileriGetir"))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();

                    aktifKategoriler = JsonConvert.DeserializeObject<List<Category>>(apiCevap);
                }

                using (var cevap = await httpClient.GetAsync($"{uri}/api/Supplier/AktifTedarikcileriGetir"))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();

                    aktifTedarikciler = JsonConvert.DeserializeObject<List<Supplier>>(apiCevap);
                }
            }

            ViewBag.AktifKategoriler = new SelectList(aktifKategoriler, "ID", "CategoryName");
            ViewBag.AktifTedarikciler = new SelectList(aktifTedarikciler, "ID", "SupplierName");

            return View(); //sadece ekleme View'ını gösterirken ViewBag'ler ile select listlere aktif kategoriler ve aktif tedarikçileri göndermek istiyoruz.
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(Product product)
        {
            product.IsActive = true;

            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(product), Encoding.UTF8, "application/json"); //Türkçe karakterini destekleyen json oluşturduk.

                using (var cevap = await httpClient.PostAsync($"{uri}/api/Product/UrunEkle", content))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();


                }
            }

            return RedirectToAction("Index");
        }

        static Product updatedProduct;
        [HttpGet]
        public async Task<IActionResult> UpdateProduct(int id) //id ile iligili kategoriyi bul ve viewde göster
        {
           

            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{uri}/api/Product/IdyeGoreUrunGetir/{id}"))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();

                    updatedProduct = JsonConvert.DeserializeObject<Product>(apiCevap);
                }

                using (var cevap = await httpClient.GetAsync($"{uri}/api/Category/AktifKategorileriGetir"))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();

                    aktifKategoriler = JsonConvert.DeserializeObject<List<Category>>(apiCevap);
                }

                using (var cevap = await httpClient.GetAsync($"{uri}/api/Supplier/AktifTedarikcileriGetir"))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();

                    aktifTedarikciler = JsonConvert.DeserializeObject<List<Supplier>>(apiCevap);
                }
            }

            ViewBag.AktifKategoriler = new SelectList(aktifKategoriler, "ID", "CategoryName");
            ViewBag.AktifTedarikciler = new SelectList(aktifTedarikciler, "ID", "SupplierName");

            return View(updatedProduct);


            
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProduct(Product guncelUrun)
        {


            using (var httpClient = new HttpClient())
            {
                guncelUrun.AddedDate = updatedProduct.AddedDate;
                guncelUrun.IsActive = updatedProduct.IsActive;
                StringContent content = new StringContent(JsonConvert.SerializeObject(guncelUrun), Encoding.UTF8, "application/json"); //Türkçe karakterini destekleyen json oluşturduk.

                using (var cevap = await httpClient.PutAsync($"{uri}/api/Product/UrunGuncelle/{guncelUrun.ID}", content))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();


                }
            }

            return RedirectToAction("Index");
        }
    }
}
