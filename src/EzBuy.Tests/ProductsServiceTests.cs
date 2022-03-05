using EzBuy.Data;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Xunit;
using System.Collections.Generic;
using System.Linq;
using EzBuy.Services;
using EzBuy.InputModels.AddEdit;
using EzBuy.Models;
using EzBuy.Services.Contracts;
using CloudinaryDotNet;

namespace EzBuy.Tests
{
    public class ProductsServiceTests
    {
        private readonly EzBuyContext context;
        private readonly ProductsService productsService;
        private readonly TestSettings settings = new TestSettings();
        private readonly ICloudinaryService cloudinaryService;
        private readonly Cloudinary cloudinary;

        public ProductsServiceTests(ICloudinaryService cloudinaryService, Cloudinary cloudinary)
        {
            this.context = settings.InitializeDatabase().GetAwaiter().GetResult();
            this.cloudinaryService = cloudinaryService;
            this.cloudinary = cloudinary;
            this.productsService = new ProductsService(context, cloudinaryService, cloudinary);
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
                Category = 1
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
        public void FindTagsReturnsUniqeTags()
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
            var tags = productsService.FindTags("magnificent,bold,dope,cool,rad,idk,dope,dope,dope");
            NUnit.Framework.Assert.NotNull(tags);
            NUnit.Framework.Assert.AreEqual(2, tags.Count);
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
                Category = 1
            };
            this.productsService.AddProductAsync(input, new User(), "");
            NUnit.Framework.Assert.AreEqual(1, this.context.Products.Count());
            //I shall return
        }
        [Fact]
        public void AddNonexistentTagsAdsNonexistentTags()
        {
            AddProductInputModel input = new AddProductInputModel()
            {
                Name = "AwesomeProduct",
                Manufacturer = "Awesome INC",
                Tags = "cool,awesome,nice",
                Price = 20,
                Description = "Wow so frikin cool",
                Category = 1
            };
            this.productsService.AddProductAsync(input, new User(), "");
            this.productsService.AddNonexistentTags("incredible,nice,vroom,cool,typical,awesome");
           NUnit.Framework.Assert.AreEqual(6, this.context.Tags.Count());
            var tag = this.context.Tags.FirstOrDefault(x => x.Name == "awesome");
            NUnit.Framework.Assert.AreEqual(tag.Id,2);
            tag = this.context.Tags.FirstOrDefault(x => x.Name == "typical");
            NUnit.Framework.Assert.AreEqual(tag.Id, 6);
        }
        [Fact]
        public void AddTagsToProductsCreatesConnections()
        {
            var tags = new List<Tag>();
            tags.Add(new Tag
            {
                Id = 1,
                Name = "cool"
            });
            tags.Add(new Tag
            {
                Id = 2,
                Name = "rad"
            });
            
            AddProductInputModel input = new AddProductInputModel()
            {
                Name = "AwesomeProduct",
                Manufacturer = "Awesome INC",
                Tags = "nice",
                Price = 20,
                Description = "Wow so frikin cool",
                Category = 1
            };
            this.productsService.AddNonexistentTags("cool,rad");
            this.productsService.AddProductAsync(input, new User(), "");

            var product= context.Products.FirstOrDefault(x => x.Name == "AwesomeProduct");
            this.productsService.AddTagsToProduct(tags, product);
            NUnit.Framework.Assert.AreEqual(3, product.Tags.Count());
        }
        [Fact]
        public void DeleteProductDeletes()
        {
            AddProductInputModel input = new AddProductInputModel()
            {
                Name = "AwesomeProduct",
                Manufacturer = "Awesome INC",
                Tags = "nice,cool,rad",
                Price = 20,
                Description = "Wow so frikin cool",
                Category = 1
            };
            this.productsService.AddProductAsync(input, new User(), "");
            NUnit.Framework.Assert.AreEqual(3, this.context.ProductTags.Count());
            this.productsService.DeleteProduct("AwesomeProduct");
            NUnit.Framework.Assert.AreEqual(0, this.context.Products.Count());
            NUnit.Framework.Assert.AreEqual(0, this.context.ProductTags.Count());
        }
        

    }
}