using CloudinaryDotNet;
using EzBuy.Data;
using EzBuy.Services;
using EzBuy.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ezbuy.Test.Services
{
    public class ProductsServiceTest : BaseDbTest
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
        public void TestSetup()
        {
            Assert.NotNull(this.productsService);
            Assert.NotNull(this.context);
            Assert.IsTrue(this.context.Database.CanConnect());
        }

        [Test]
        public void TestAddingProducts()
        {
            PopulateDbWithProducts(count: 2).GetAwaiter().GetResult();

            Assert.That(this.context.Products.Count(), Is.EqualTo(2));
        }
    }
}
