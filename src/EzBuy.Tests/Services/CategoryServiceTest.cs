using EzBuy.Data;
using EzBuy.Services;
using EzBuy.ViewModels.Category;
using NUnit.Framework;

namespace EzBuy.Tests.Services
{
    public class CategoryServiceTest
    {
        private readonly EzBuyContext context;
        private readonly CategoryService categoryService;
        private readonly TestSettings settings = new TestSettings();

        public CategoryServiceTest()
        {
            this.context = settings.InitializeDatabase().GetAwaiter().GetResult();
            this.categoryService = new CategoryService(context);
        }

        [Test]
        public void GetAllCategoriesTest()
        {
            var categories = this.categoryService.GetAllCategories();
            Assert.That(categories, Is.InstanceOf<CategoryOnAddFormViewModel>());
        }
    }
}
