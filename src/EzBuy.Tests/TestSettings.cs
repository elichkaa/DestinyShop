using EzBuy.Data;
using EzBuy.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzBuy.Tests
{
    public class TestSettings
    {
        private SqliteConnection connection;
        private EzBuyContext context;

        public async Task<EzBuyContext> InitializeDatabase()
        {
            this.connection = new SqliteConnection("DataSource=:memory:");
            this.connection.Open();
            var options = new DbContextOptionsBuilder<EzBuyContext>().UseSqlite(this.connection);
            this.context = new EzBuyContext(options.Options);

            await this.context.Database.EnsureDeletedAsync();
            await this.context.Database.EnsureCreatedAsync();

            return this.context;
        }
        public async Task MakeProducts(int count)
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
        public async Task MakeManufacurers(int count)
        {
            var manufacturers = new List<Manufacturer>();
            for (int i = 1; i <= count; i++)
            {
                manufacturers.Add(new Manufacturer()
                {
                    Id = i,
                    Name = $"Manufacturer{i}"
                });
            }

            await context.Manufacturers.AddRangeAsync(manufacturers);
            await context.SaveChangesAsync();
        }

    }
}
