using EzBuy.Data;
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
    public class ProductsServiceTests: IClassFixture<Cloudinary>
    {
        private readonly EzBuyContext context;
        private ProductsService productsService;
        private readonly TestSettings settings = new TestSettings();
        private readonly ICloudinaryService cloudinaryService;
        private readonly Cloudinary cloudinary;

        public ProductsServiceTests()
        {
            this.context = settings.InitializeDatabase().GetAwaiter().GetResult();
        }

        [Fact]
        public void AddProductComponentsAddsComponents()
        {
            this.productsService = new ProductsService(context, cloudinaryService, cloudinary);
            AddProductInputModel input = new AddProductInputModel()
            {
                Name = "AwesomeProduct",
                Manufacturer = "Awesome INC",
                Tags = "cool,awesome,nice",
                Price = 20,
                Description = "Wow so frikin cool",
                Category = 1
            };
            this.productsService.AddProductComponents(input);
            var manufacturer = context.Manufacturers.FirstOrDefault(x => x.Name == "Awesome INC");
            var tag1 = context.Tags.FirstOrDefault(x => x.Name == "cool");
            var tag2 = context.Tags.FirstOrDefault(x => x.Name == "awesome");
            var tag3 = context.Tags.FirstOrDefault(x => x.Name == "nice");

            Assert.NotNull(manufacturer);
            Assert.Equal("Awesome INC", manufacturer.Name);

            Assert.NotNull(tag1);
            Assert.Equal("cool", tag1.Name);

            Assert.NotNull(tag2);
            Assert.Equal("awesome", tag2.Name);

            Assert.NotNull(tag3);
            Assert.Equal("nice", tag3.Name);
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

            Assert.NotNull(tags);
            Assert.Equal(2, tags.Count);
            Assert.Equal("dope", tags.ToList()[0].Name);
            Assert.Equal("rad", tags.ToList()[1].Name);
        }

        [Fact]
        public void FindTagsReturnsUniqeTags()
        {
            this.context.Tags.Add(new Tag
            {
                Id = 1,
                Name = "dope",
            });
            this.context.SaveChanges();
            this.context.Tags.Add(new Tag
            {
                Id = 2,
                Name = "rad"
            });
            this.context.SaveChanges();
            var tags = productsService.FindTags("magnificent,bold,dope,cool,rad,idk,dope,dope,dope");
            Assert.NotNull(tags);
            Assert.Equal(2, tags.Count);
        }

        [Fact]
        public void AddProductAddsProducts()
        {
            Assert.Equal(0, this.context.Products.Count());
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
            Assert.Equal(1, this.context.Products.Count());
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
            Assert.Equal(6, this.context.Tags.Count());
            var tag = this.context.Tags.FirstOrDefault(x => x.Name == "awesome");
            tag = this.context.Tags.FirstOrDefault(x => x.Name == "typical");
            Assert.Equal(2, tag.Id);
            tag = this.context.Tags.FirstOrDefault(x => x.Name == "typical");
            Assert.Equal(6, tag.Id);
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
            Assert.Equal(3, product.Tags.Count());
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
            Assert.Equal(3, this.context.ProductTags.Count());
            //this.productsService.DeleteProduct("AwesomeProduct");
            //NUnit.Framework.Assert.AreEqual(0, this.context.Products.Count());
            //NUnit.Framework.Assert.AreEqual(0, this.context.ProductTags.Count());
        }
        

    }
}