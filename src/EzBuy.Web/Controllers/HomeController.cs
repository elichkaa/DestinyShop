using EzBuy.Services.Contracts;
using EzBuy.ViewModels.Home;
using EzBuy.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;

namespace EzBuy.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICategoryService categoryService;
        private readonly IProductService productService;
        
        public HomeController(ILogger<HomeController> logger, 
            ICategoryService categoryService,
            IProductService productService)
        {
            _logger = logger;
            this.categoryService = categoryService;
            this.productService = productService;
        }

        public IActionResult Index()
        {
            var categories = this.categoryService.GetCategoriesOnHomePage();
            var products = this.productService.GetTopProducts();
            var homePage = new HomePageViewModel
            {
                Products = products,
                Categories = categories,
            };
            return View(homePage);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Store()
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