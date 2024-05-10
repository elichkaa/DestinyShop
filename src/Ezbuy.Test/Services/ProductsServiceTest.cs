using CloudinaryDotNet;
using EzBuy.Models;
using EzBuy.Services;
using EzBuy.Services.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Ezbuy.Test.Services
{
    public class ProductsServiceTest : BaseDbTest
    {
        private ProductsService productsService;
        private readonly ICloudinaryService cloudinaryService;
        private readonly Cloudinary cloudinary;
        private const int SIZE = 5;

        [OneTimeSetUp]
        public async Task Setup()
        {
            this.productsService = new ProductsService(this.context, this.cloudinaryService, this.cloudinary);
            await PopulateDbWithProducts(count: SIZE);
            await PopulateDbWithCategories(count: SIZE);
        }

        [Test]
        public void TestSetup()
        {
            Assert.NotNull(this.productsService);
            Assert.NotNull(this.context);
            Assert.IsTrue(this.context.Database.CanConnect());
        }

        [Test]
        public void CorrectCountAfterAddingObjects()
        {
            Assert.That(this.context.Products.Count(), Is.EqualTo(SIZE));
            Assert.That(this.context.Categories.Count(), Is.EqualTo(SIZE));
        }

        [Test]
        public void FindMethodsReturnNothingWithEmptyArg()
        {
            Assert.IsEmpty(this.productsService.FindTags(String.Empty));
            Assert.IsEmpty(this.productsService.FindTags(null));
            Assert.IsNull(this.productsService.FindManufacturer(String.Empty));
            Assert.IsNull(this.productsService.FindManufacturer(null));
            Assert.IsEmpty(this.productsService.FindProductTags(new Product()));
            Assert.IsEmpty(this.productsService.FindProductTags(null));
        }

        [Test]
        public void GetNonExistingCategoryReturnsNullAndThenTrueAfterChecking()
        {
            Assert.Null(this.productsService.GetCategory(-1));
            Assert.False(this.productsService.CheckIfEntityExists<Category>(null));
            Assert.False(this.productsService.CheckIfEntityExists<Category>("TestCategory5"));
        }

        [Test]
        public void GetExistingCategorySuccess()
        {
            Assert.NotNull(this.productsService.GetCategory(1));
            Assert.True(this.productsService.CheckIfEntityExists<Category>("TestCategory0"));
        }

        [Test]
        public void FindTagsReturnsUniqueExistingTags()
        {
            var tags = productsService.FindTags("Tag0,Tag1,Tag0");

            Assert.NotNull(tags);
            Assert.That(tags.Count, Is.EqualTo(2));
            Assert.That(tags.ToList()[0].Name, Is.EqualTo("Tag0"));
            Assert.That(tags.ToList()[1].Name, Is.EqualTo("Tag1"));
        }

        [Test]
        public void AddNonexistentTagsAdsNonexistentTags()
        {
            this.productsService.AddNonexistentTags("incredible,nice,vroom,cool,typical,awesome");

            var incredibleTag = this.context.Tags.FirstOrDefault(x => x.Name == "incredible");
            var niceTag = this.context.Tags.FirstOrDefault(x => x.Name == "nice");
            var vroomTag = this.context.Tags.FirstOrDefault(x => x.Name == "vroom");
            var coolTag = this.context.Tags.FirstOrDefault(x => x.Name == "cool");
            var awesomeTag = this.context.Tags.FirstOrDefault(x => x.Name == "awesome");
            var typicalTag = this.context.Tags.FirstOrDefault(x => x.Name == "typical");

            Assert.NotNull(awesomeTag);
            Assert.That(awesomeTag.Name, Is.EqualTo("awesome"));
            Assert.NotNull(typicalTag);
            Assert.That(typicalTag.Name, Is.EqualTo("typical"));

            this.context.Tags.RemoveRange(new List<Tag> { incredibleTag, niceTag, vroomTag, coolTag, awesomeTag, typicalTag });
        }

        [Test]
        public async Task AddTagsToProductsCreatesCorrectRelationshipBetweenProductAndTag()
        {
            this.productsService.AddNonexistentTags("incredible,nice");
            var tags = this.context.Tags.ToList().TakeLast(2).ToList();
            var productFromDb = await context.Products.FirstOrDefaultAsync(x => x.Name == "Product0");
            Assert.NotNull(productFromDb);
            await this.productsService.AddTagsToProduct(tags, productFromDb);
            Assert.That(productFromDb.Tags.Count(), Is.EqualTo(3));
            Assert.That(productFromDb.Tags.Select(x => x.TagId).TakeLast(2).ToList(), Is.EqualTo(tags.Select(x => x.Id).ToList()));
            var incredibleTag = this.context.Tags.FirstOrDefault(x => x.Name == "incredible");
            var niceTag = this.context.Tags.FirstOrDefault(x => x.Name == "nice");
            this.context.Tags.RemoveRange(new List<Tag> { incredibleTag, niceTag });
        }
    }
}
