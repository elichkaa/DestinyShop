using EzBuy.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EzBuy.Data
{
    public class EzBuyContext : IdentityDbContext<User, IdentityRole<Guid>, Guid, IdentityUserClaim<Guid>, IdentityUserRole<Guid>, IdentityUserLogin<Guid>, IdentityRoleClaim<Guid>, IdentityUserToken<Guid>>
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

            modelBuilder.Entity<User>(c =>
            {
                c.Property(p => p.CreatedOn).HasDefaultValueSql("GETDATE()").ValueGeneratedOnAdd();
                c.Property(p => p.ModifiedOn).HasDefaultValueSql("GETDATE()").ValueGeneratedOnAddOrUpdate();
                c.Property(p => p.DeletedOn).HasDefaultValue(null);
                c.Property(p => p.IsDeleted).HasDefaultValue(false);
                c.Property(p => p.EzBucks).HasColumnType("decimal(5,2)").HasPrecision(5).HasDefaultValue(0);
                c.HasOne(a => a.Cart).WithOne(b => b.User).HasForeignKey<Cart>(b => b.UserId);
            });

            modelBuilder.Entity<Product>(p =>
            {
                p.Property(x => x.Price).HasColumnType("decimal(5,2)").HasPrecision(5).HasDefaultValue(0);
            });

            modelBuilder.Entity<ProductTags>()
            .HasKey(bc => new { bc.ProductId, bc.TagId });

            modelBuilder.Entity<Product>()
            .HasMany(e => e.Carts)
            .WithMany(e => e.Products)
            .UsingEntity<CartProduct>();

            modelBuilder.Entity<CartProduct>()
            .HasKey(i => new { i.ProductId, i.CartId });

            modelBuilder.Entity<CartProduct>()
                .HasOne(i => i.Product)
                .WithMany(i => i.CartProducts)
                .HasForeignKey(i => i.CartId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CartProduct>()
                .HasOne(i => i.Cart)
                .WithMany(i => i.CartProducts)
                .HasForeignKey(i => i.ProductId)
                .OnDelete(DeleteBehavior.NoAction);
        }

        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartProduct> CartProducts { get; set; }
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