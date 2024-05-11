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
            Assert.IsEmpty(this.productsService.GetTags(String.Empty));
            Assert.IsEmpty(this.productsService.GetTags(null));
            Assert.IsNull(this.productsService.GetManufacturer(String.Empty));
            Assert.IsNull(this.productsService.GetManufacturer(null));
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
        public void GetTagsReturnsUniqueExistingTags()
        {
            var tags = productsService.GetTags("Tag0,Tag1,Tag0");

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

            Assert.NotNull(incredibleTag);
            Assert.That(incredibleTag.Name, Is.EqualTo("incredible"));
            Assert.NotNull(niceTag);
            Assert.That(niceTag.Name, Is.EqualTo("nice"));
            Assert.NotNull(vroomTag);
            Assert.That(vroomTag.Name, Is.EqualTo("vroom"));
            Assert.NotNull(coolTag);
            Assert.That(coolTag.Name, Is.EqualTo("cool"));
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

            var incredibleTag = await this.context.Tags.FirstOrDefaultAsync(x => x.Name == "incredible");
            var niceTag = await this.context.Tags.FirstOrDefaultAsync(x => x.Name == "nice");
            await this.context.Tags.Where(x => x.Equals(niceTag) || x.Equals(incredibleTag)).ExecuteDeleteAsync();
            Assert.False(await this.context.ProductTags.Select(x => x.Product == productFromDb && x.Tag == incredibleTag).FirstOrDefaultAsync());
            Assert.False(await this.context.ProductTags.Select(x => x.Product == productFromDb && x.Tag == niceTag).FirstOrDefaultAsync());
        }

        [Test]
        public void AddProductComponentsWithInvalidArguments()
        {
            Assert.ThrowsAsync<ArgumentException>(async () => await this.productsService.AddProductComponents(null));
        }
    }
}
