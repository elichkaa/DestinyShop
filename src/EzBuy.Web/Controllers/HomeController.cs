using EzBuy.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace EzBuy.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Homepage()
        {
            return View();
        }
        public IActionResult Checkout()
        {
            return View();
        }

        public IActionResult Product()
        {
            return View();
        }
        public IActionResult Store()
        {
            return View();
        }

        public IActionResult Blankpage()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}