using EzBuy.Models;
using Microsoft.EntityFrameworkCore;

namespace EzBuy.Data
{
    class EzBuyContext : DbContext
    {
        public EzBuyContext()
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=.;Database=EzBuyDB;Trusted_Connection=True;");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CartProducts>()
            .HasKey(bc => new { bc.ProductId, bc.CartId });

            modelBuilder.Entity<ProductTags>()
            .HasKey(bc => new { bc.ProductId, bc.TagId });

            modelBuilder.Entity<User>()
            .HasOne(a => a.Cart)
            .WithOne(b => b.User)
            .HasForeignKey<Cart>(b => b.UserId);

            modelBuilder.Entity<Product>()
            .HasOne(a => a.CoverImage)
            .WithOne(b => b.Product)
            .HasForeignKey<CoverImage>(b => b.ProductId);
        }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartProducts> CartProducts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<CoverImage> CoverImages { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Manufacturer> Manufacturers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductTags> ProductTags { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Sale> Sales { get; set; }
    }
}