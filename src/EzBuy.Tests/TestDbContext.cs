using EzBuy.Models;
using Microsoft.EntityFrameworkCore;

namespace EzBuy.Tests
{
    public class TestDbContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }

        public TestDbContext(DbContextOptions<TestDbContext> options) : base(options) {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(TestDbContext).Assembly);
        }
    }
}
