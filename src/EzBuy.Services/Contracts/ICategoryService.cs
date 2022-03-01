using EzBuy.ViewModels.Category;

namespace EzBuy.Services.Contracts
{
    public interface ICategoryService
    {
        public List<CategoryOnAddFormViewModel> GetAllCategories();
    }
}
