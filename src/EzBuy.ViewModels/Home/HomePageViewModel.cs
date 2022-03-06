using EzBuy.ViewModels.Category;
using EzBuy.ViewModels.Products;

namespace EzBuy.ViewModels.Home
{
    public class HomePageViewModel
    {
        public List<CategoryOnHomePageViewModel> Categories { get; set; }
        public List<ProductOnAllPageViewModel> Products { get; set; }
    }
}
