using Microsoft.AspNetCore.Mvc;
using EzBuy.Web.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;

namespace EzBuy.Web.Controllers
{
    [Authorize]
    public class ProductsController : Controller
    {
        public ILogger<ProductsController> _logger { get; set; }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Add()
        {
            return View();
        }
        public async Task<IActionResult> Edit()
        {
            return View();
        }
        public async Task<IActionResult> Delete()
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
