using EzBuy.Data;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Xunit;
using System.Collections.Generic;
using System.Linq;
using EzBuy.Services;
using EzBuy.InputModels.AddEdit;
using EzBuy.Models;

namespace EzBuy.Tests
{
    public class ProductsServiceTests
    {
        private readonly EzBuyContext context;
        private readonly ProductsService productsService;
        private readonly TestSettings settings = new TestSettings();

        public ProductsServiceTests()
        {
            this.context = settings.InitializeDatabase().GetAwaiter().GetResult();
            this.productsService = new ProductsService(context);
        }

        [Fact]
        public void AddProductComponentsAddsComponents()
        {
            AddProductInputModel input = new AddProductInputModel()
            {
                Name = "AwesomeProduct",
                Manufacturer = "Awesome INC",
                Tags = "cool,awesome,nice",
                Price = 20,
                Description = "Wow so frikin cool",
                Category = "Gaming"
            };
            productsService.AddProductComponents(input);
            var manufacturer = context.Manufacturers.FirstOrDefault(x => x.Name == "Awesome INC");
            var tag1 = context.Tags.FirstOrDefault(x => x.Name == "cool");
            var tag2 = context.Tags.FirstOrDefault(x => x.Name == "awesome");
            var tag3 = context.Tags.FirstOrDefault(x => x.Name == "nice");
            NUnit.Framework.Assert.NotNull(manufacturer);
            NUnit.Framework.Assert.AreEqual(manufacturer.Name, "Awesome INC");

            NUnit.Framework.Assert.NotNull(tag1);
            NUnit.Framework.Assert.AreEqual(tag1.Name, "cool");

            NUnit.Framework.Assert.NotNull(tag2);
            NUnit.Framework.Assert.AreEqual(tag2.Name, "awesome");

            NUnit.Framework.Assert.NotNull(tag3);
            NUnit.Framework.Assert.AreEqual(tag3.Name, "nice");
        }

        [Fact]
        public void FindTagsReturnsExistingTags()
        {
            this.context.Tags.Add(new Tag
            {
                Id = 1,
                Name = "dope"
            });
            this.context.SaveChanges();
            this.context.Tags.Add(new Tag
            {
                Id = 2,
                Name = "rad"
            });
            this.context.SaveChanges();
            var tags = productsService.FindTags("magnificent,bold,dope,cool,rad,idk");
            NUnit.Framework.Assert.NotNull(tags);
            NUnit.Framework.Assert.AreEqual(2, tags.Count);
            NUnit.Framework.Assert.AreEqual(tags.ToList()[0].Name, "dope");
            NUnit.Framework.Assert.AreEqual(tags.ToList()[1].Name, "rad");
        }
        [Fact]
        public void AddProductAddsProducts()
        {
            NUnit.Framework.Assert.AreEqual(0, this.context.Products.Count());
            AddProductInputModel input = new AddProductInputModel()
            {
                Name = "AwesomeProduct",
                Manufacturer = "Awesome INC",
                Tags = "cool,awesome,nice",
                Price = 20,
                Description = "Wow so frikin cool",
                Category = "Gaming"
            };
            this.productsService.AddProduct(input, new User());
            NUnit.Framework.Assert.AreEqual(1, this.context.Products.Count());
            //I shall return
        }

    }
}