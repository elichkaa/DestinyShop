using Microsoft.AspNetCore.Mvc;
using EzBuy.Web.Models;
using System.Diagnostics;
using EzBuy.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using EzBuy.InputModels.AddEdit;
using Microsoft.AspNetCore.Identity;
using EzBuy.Models;

namespace EzBuy.Web.Controllers
{
    [Authorize]
    public class ProductsController : Controller
    {
        private readonly ICategoryService categoriesService;
        private readonly IProductService productService;
        private readonly UserManager<User> userManager;
        private readonly IWebHostEnvironment webHostEnvironment;

        public ILogger<ProductsController> _logger { get; set; }

        public ProductsController(
            ICategoryService categoriesService, 
            IProductService productService,
            UserManager<User> userManager,
            IWebHostEnvironment webHostEnvironment)
        {
            this.categoriesService = categoriesService;
            this.productService = productService;
            this.userManager = userManager;
            this.webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        [HttpGet]
        public IActionResult Add()
        {
            var categories = this.categoriesService.GetAllCategories();
            this.ViewBag.Categories = categories;
            return this.View(new AddProductInputModel());
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Add(AddProductInputModel input)
        {
            var categories = this.categoriesService.GetAllCategories();
            this.ViewBag.Categories = categories;
            if (!ModelState.IsValid)
            {
                this.ModelState.AddModelError(string.Empty, "Invalid arguments");
                return this.View(input);
            }

            var currentUser = await this.userManager.GetUserAsync(this.User);
            try
            {
                int newProductId = await this.productService.AddProductAsync(input, currentUser, $"{this.webHostEnvironment.WebRootPath}/img/");
            }
            catch (Exception ex)
            {
                this.ModelState.AddModelError(string.Empty, ex.Message);
                return this.View(input);
            }

            return Redirect("/");
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Edit(int productId)
        {
            var filledModel = await this.productService.GetFilledProductById(productId);
            ViewData["Filled"] = filledModel;
            var categories = this.categoriesService.GetAllCategories();
            this.ViewBag.Categories = categories;
            return this.View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Edit(EditProductInputModel input, int productId)
        {
            var filledModel = await this.productService.GetFilledProductById(productId);
            ViewData["Filled"] = filledModel;
            input.Id = filledModel.Id;
            var categories = this.categoriesService.GetAllCategories();
            this.ViewBag.Categories = categories;

            if (!ModelState.IsValid)
            {
                this.ModelState.AddModelError(string.Empty, "Invalid arguments");
                return this.View(input);
            }

            try
            {
                await this.productService.EditProductAsync(input, $"{this.webHostEnvironment.WebRootPath}/img/");
            }
            catch (Exception ex)
            {
                this.ModelState.AddModelError(string.Empty, ex.Message);
                return this.View(input);
            }

            return this.RedirectToAction("Overview");
        }


        public async Task<IActionResult> Delete()
        {
            var id = TempData["productId"];
            await this.productService.DeleteProduct((int)id);
            return this.RedirectToAction("Overview");
        }

        public async Task<IActionResult> DeleteImage()
        {
            await this.productService.DeleteProductImageByPathAsync(TempData["path"].ToString());
            return this.RedirectToAction("Edit", "Products", new {productId = TempData["productId"]});
        }

        public IActionResult ProductModal(int productId)
        {
            TempData["productId"] = productId;
            return PartialView("_DeleteProduct");
        }

        public IActionResult ImageModal(string path, int productId)
        {
            TempData["path"] = path;
            TempData["productId"] = productId;
            return PartialView("_DeleteImage");
        }

        public IActionResult Overview()
        {
            var products = productService.GetProductsByUserId(this.User.Identity.Name);
            return View(products);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
