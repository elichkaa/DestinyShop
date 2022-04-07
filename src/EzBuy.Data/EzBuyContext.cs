using EzBuy.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace EzBuy.Data
{
    public class EzBuyContext : IdentityDbContext<User, IdentityRole<string>, string, IdentityUserClaim<string>, IdentityUserRole<string>, IdentityUserLogin<string>, IdentityRoleClaim<string>, IdentityUserToken<string>>
    {

        public EzBuyContext(DbContextOptions<EzBuyContext> options)
            : base(options)
        {
        }

        public EzBuyContext()
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CartProducts>()
            .HasKey(bc => new { bc.ProductId, bc.CartId });

            modelBuilder.Entity<ProductTags>()
            .HasKey(bc => new { bc.ProductId, bc.TagId });

            modelBuilder.Entity<User>()
            .HasOne(a => a.Cart)
            .WithOne(b => b.User)
            .HasForeignKey<Cart>(b => b.UserId);
        }

        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartProducts> CartProducts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Manufacturer> Manufacturers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductTags> ProductTags { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Sale> Sales { get; set; }
        

        public DbSet<IdentityRole> IdentityRoles { get; set; }
    }
}