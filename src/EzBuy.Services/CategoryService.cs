using EzBuy.Data;
using EzBuy.Services.Contracts;
using EzBuy.ViewModels.Category;

namespace EzBuy.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly EzBuyContext context;

        public CategoryService(EzBuyContext context)
        {
            this.context = context;
        }

        public List<CategoryOnAddFormViewModel> GetAllCategories()
        {
            return this.context.Categories.
                Select(x => new CategoryOnAddFormViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                }).ToList();
        }

        public List<CategoryOnHomePageViewModel> GetCategoriesOnHomePage()
        {
            var categories = new List<CategoryOnHomePageViewModel>();
            var indexes = new List<int>() { 6,19,29};
            var images = new List<string>()
            {
                "https://res.cloudinary.com/ezbuy/image/upload/v1646551860/clean_qgoqoi.jpg",
                "https://res.cloudinary.com/ezbuy/image/upload/v1646552139/art3_ubnpdl.png",
                "https://res.cloudinary.com/ezbuy/image/upload/v1649584595/garfield1.jpg"
            };
            for (int i = 0; i < 3; i++)
            {
                var category = this.context.Categories.FirstOrDefault(x => x.Id == indexes[i]);
                categories.Add(new CategoryOnHomePageViewModel
                {
                    Name = category.Name,
                    Id = category.Id,
                    Image = images[i]
                });
            }
            return categories;
        }

        public List<CategoryOnShopPageViewModel> GetCategoriesOnShopPage()
        {
            return this.context.Categories.
                Select(x => new CategoryOnShopPageViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    ProductCount = x.Products == null ? 0 : x.Products.Count()
                }).ToList();
        }
    }
}
