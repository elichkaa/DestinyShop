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

        public async Task PopulateDbWithProducts(int count)
        {
            var products = new List<Product>();
            for (int i = 1; i <= count; i++)
            {
                DateTime? dateTime = new DateTime(2020, 12, i);
                products.Add(new Product()
                {
                    Id = i,
                    Name = $"Product{i}",
                    DateListed = dateTime,
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

        public void Dispose() { 
            this.context.Dispose();
        }
    }
}
