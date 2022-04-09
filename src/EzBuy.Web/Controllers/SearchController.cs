using EzBuy.InputModels.Search;
using EzBuy.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace EzBuy.Web.Controllers
{
    public class SearchController : Controller
    {
       
            private readonly IProductService productsService;


            public SearchController(IProductService productsService)
            {
                this.productsService = productsService;
            }

            public IActionResult Index()
            {
                return this.View();
            }

            public IActionResult Product()
            {
                return this.View();
            }

            public IActionResult ProductResults(SearchProductInputModel input)
            {
                if (!ModelState.IsValid)
                {
                    this.ModelState.AddModelError(string.Empty, "Product name should be 2 symbols minimum.");
                    this.TempData["ModelState"] = this.ModelState.Root.Errors[0].ErrorMessage;
                    return this.Redirect("/Search/Product/");
                }
                var products = productsService.SearchProducts(input);
                return this.View(products);
            }


        }
    }

