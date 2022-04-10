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

        public IActionResult Product()
        {
            return this.View();
        }

        public IActionResult ProductResults(SearchProductInputModel input)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(x => x.ErrorMessage).ToList();
                this.TempData["ModelState"] = errors;
                return this.Redirect("/Search/Product/");
            }
            var products = productsService.SearchProducts(input);
            return this.View(products);
        }
    }
}

