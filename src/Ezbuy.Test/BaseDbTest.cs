using EzBuy.Data;
using EzBuy.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Ezbuy.Test
{
    public class BaseDbTest
    {
        private SqliteConnection connection;
        protected EzBuyContext context;

        public BaseDbTest() {
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
        /// Adds <c>count</c> products to the database. Ensures there are no duplicate id's.
        /// </summary>
        /// <param name="count">number of products to add to database</param>
        public async Task PopulateDbWithProducts(int count)
        {
            int productsAlreadyInDb = await context.Products.CountAsync();
            var products = new List<Product>();
            for (int i = productsAlreadyInDb + 1; i <= count; i++)
            {
                products.Add(new Product()
                {
                    Id = i,
                    Name = $"Product{i}",
                    DateListed = new DateTime(2020, 12, 1),
                    Tags = new List<ProductTags>()
                    {
                        new ProductTags()
                        {
                            Tag = new Tag()
                            {
                                Id = i,
                                Name =  $"Tag{i}"
                            }
                        }
                    }
                });
            }

            await context.Products.AddRangeAsync(products);
            await context.SaveChangesAsync();
        }
    }
}
