using EzBuy.Data;
using EzBuy.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Ezbuy.Test
{
    public class BaseDbTest
    {
        protected SqliteConnection connection;
        protected EzBuyContext context;

        public BaseDbTest()
        {
            this.connection = new SqliteConnection("DataSource=:memory:");
            this.connection.Open();
            var options = new DbContextOptionsBuilder<EzBuyContext>().UseSqlite(this.connection);
            this.context = new EzBuyContext(options.Options);
            this.context.Database.EnsureDeleted();
            this.context.Database.EnsureCreated();
        }

        [OneTimeTearDown]
        public async Task LastTearDown()
        {
            await this.context.DisposeAsync();
            await this.connection.DisposeAsync();
        }

        /// <summary>
        /// Populates the database with objects of a specific type. Ensures there are no duplicate ids.
        /// </summary>
        /// <param name="table">table to add objects to</param
        /// <param name="newObjects">list of objects to add to database, must be of base type <c>MainEntity</c></param>
        private async Task PopulateDbWithObjectHelper<T>(DbSet<T> table, List<T> newObjects) where T : MainEntity
        {
            int objectsAlreadyInDb = await table.CountAsync();
            for (int i = 0; i < newObjects.Count; i++)
            {
                newObjects[i].Id = objectsAlreadyInDb + i + 1;
            }
            await table.AddRangeAsync(newObjects);
            await context.SaveChangesAsync();
        }

        protected async Task PopulateDbWithProducts(int count)
        {
            int tagsCount = await this.context.Tags.CountAsync();
            List<Product> products = new List<Product>();
            for (int i = 0; i < count; i++)
            {
                products.Add(new Product()
                {
                    Id = i,
                    Name = $"Product{i}",
                    DateListed = DateTime.Now,
                    Tags = new List<ProductTags>()
                    {
                        new ProductTags()
                        {
                            Tag = new Tag()
                            {
                                Id = tagsCount + i + 1,
                                Name =  $"Tag{i}"
                            }
                        }
                    }
                });
            }
            await PopulateDbWithObjectHelper(this.context.Products, products);
        }

        protected async Task PopulateDbWithCategories(int count)
        {
            List<Category> categories = new List<Category>();
            for (int i = 0; i < count; i++)
            {
                categories.Add(new Category()
                {
                    Id = i,
                    Name = $"TestCategory{i}"
                });
            }
            await PopulateDbWithObjectHelper(this.context.Categories, categories);
        }

        protected async Task PopulateDbWithTags(int count)
        {
            List<Tag> tags = new List<Tag>();
            for (int i = 0; i < count; i++)
            {
                tags.Add(new Tag
                {
                    Id = i,
                    Name = $"test{i}"
                });
            }
            await PopulateDbWithObjectHelper(this.context.Tags, tags);
        }
    }
}
