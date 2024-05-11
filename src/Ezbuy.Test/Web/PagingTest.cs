using CloudinaryDotNet;
using EzBuy.Services.Contracts;
using EzBuy.Services;

namespace Ezbuy.Test.Web
{
    public class PagingTest : BaseDbTest
    {
        private ProductsService productsService;
        private readonly ICloudinaryService cloudinaryService;
        private readonly Cloudinary cloudinary;

        [SetUp]
        public void Setup()
        {
            this.productsService = new ProductsService(this.context, this.cloudinaryService, this.cloudinary);
        }

        [Test]
        public void TestPageCountCorrectDependingOnProductCount()
        {
            PopulateDbWithProducts(count: 2).GetAwaiter().GetResult();
            Assert.That(this.productsService.GetMaxPages(), Is.EqualTo(1));

            PopulateDbWithProducts(count: 18).GetAwaiter().GetResult();
            Assert.That(this.productsService.GetMaxPages(), Is.EqualTo(3));
        }
    }
}
