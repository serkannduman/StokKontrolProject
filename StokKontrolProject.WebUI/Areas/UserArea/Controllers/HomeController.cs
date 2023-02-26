using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StokKontrolProject.Domain.Entities;

namespace StokKontrolProject.WebUI.Areas.UserArea.Controllers
{
    [Area("UserArea")]
    public class HomeController : Controller
    {
        
        public async Task<IActionResult> Index()
        {
            //Tüm kategorileri listeleyeceğiz.
            return View();
        }
    }
}
