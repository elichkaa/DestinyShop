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
    }
}
