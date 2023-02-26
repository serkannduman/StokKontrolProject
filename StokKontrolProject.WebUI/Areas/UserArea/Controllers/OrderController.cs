using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StokKontrolProject.Domain.Entities;
using System;

namespace StokKontrolProject.WebUI.Areas.UserArea.Controllers
{
    public class OrderController : Controller
    {
        string uri = "https://localhost:7235";
        public async Task<IActionResult> Index(int id)
        {
            id = 1;
            List<Order> siparisler = new List<Order>();

            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{uri}/api/Order/TumSiparisleriGetir"))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();

                    siparisler = JsonConvert.DeserializeObject<List<Order>>(apiCevap).Where(x => x.UserID == id).ToList();
                }
            }

            return View(siparisler);
        }

        public async Task<IActionResult> OrderDetail(int id)
        {
            id = 1;
            List<Order> siparisler = new List<Order>();

            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{uri}/api/Order/TumSiparisleriGetir"))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();

                    siparisler = JsonConvert.DeserializeObject<List<Order>>(apiCevap).Where(x => x.UserID == id).ToList();
                }
            }

            return View(siparisler);
        }
    }
}
