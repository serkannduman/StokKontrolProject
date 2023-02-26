using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StokKontrolProject.Domain.Entities;

namespace StokKontrolProject.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrderController : Controller
    {
        string uri = "https://localhost:7235";
        public async Task<IActionResult> Index()
        {
            //Tüm kategorileri listeleyeceğiz.

            List<Order> siparisler = new List<Order>();

            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{uri}/api/Order/TumSiparisleriGetir"))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();

                    siparisler = JsonConvert.DeserializeObject<List<Order>>(apiCevap);
                }
            }

            return View(siparisler);
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmOrder(int id)
        {

            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{uri}/api/Order/SiparisOnayla/{id}"))
                {

                }
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> CancelOrder(int id)
        {

            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{uri}/api/Order/SiparisReddet/{id}"))
                {

                }
            }

            return RedirectToAction("Index");
        }

        
    }
}
