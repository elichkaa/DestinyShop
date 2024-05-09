using CloudinaryDotNet;
using EzBuy.Data;
using EzBuy.Models;
using EzBuy.Services;
using EzBuy.Services.Contracts;
using Microsoft.EntityFrameworkCore;
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
        public void FindMethodsReturnNothingOnEmptyDb()
        {
            Assert.IsEmpty(this.productsService.FindTags(String.Empty));
            Assert.IsEmpty(this.productsService.FindTags(null));
            Assert.IsNull(this.productsService.FindManufacturer(String.Empty));
            Assert.IsNull(this.productsService.FindManufacturer(null));
            Assert.IsEmpty(this.productsService.FindProductTags(new Product()));
            Assert.IsEmpty(this.productsService.FindProductTags(null));
        }

        [Test]
        public void CorrectCountAfterAddingProducts()
        {
            PopulateDbWithProducts(count: 2).GetAwaiter().GetResult();

            Assert.That(this.context.Products.Count(), Is.EqualTo(2));
        }

        [Test]
        public void GetNonExistingCategoryReturnsNull()
        {
            Assert.Null(this.productsService.GetCategory(-1));
        }

        [Test]
        public async Task GetExistingCategorySuccess()
        {
            PopulateDbWithCategories(count: 1).GetAwaiter().GetResult();
            Assert.That(await this.context.Categories.CountAsync(), Is.EqualTo(1));
            Assert.NotNull(this.productsService.GetCategory(1));
        }
    }
}
