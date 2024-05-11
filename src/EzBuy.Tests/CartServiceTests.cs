using EzBuy.Data;
using EzBuy.Models;
using EzBuy.Services;
using EzBuy.ViewModels.Category;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzBuy.Tests
{
    public class CartServiceTests
    {
        private readonly EzBuyContext context;
        private readonly CartService cartService;
        private readonly TestSettings settings = new TestSettings();

        public CartServiceTests()
        {
            this.context = settings.InitializeDatabase().GetAwaiter().GetResult();
            this.cartService = new CartService(context);
        }

        [Test]
        public void CreateUserCartCreatesCart()
        {
            var user = new User { UserName = "xXPimpXx" };
            this.context.Users.Add(user);
            this.context.SaveChanges();
            user = this.context.Users.FirstOrDefault(x => x.UserName == "xXPimpXx");
            Assert.That(user, Is.Not.Null);
            this.cartService.CreateUsersCart(user);
            Assert.That(this.context.Carts.Count(), Is.EqualTo(1));
            var cart = this.context.Carts.FirstOrDefault(x => x.Id == 1);
            Assert.That(cart, Is.Not.Null);
            Assert.That(cart.User.UserName, Is.EqualTo("xXPimpXx"));
        }

        [Test]
        public void AddProductTOCartCreatesConnections()
        {
            var cart = new Cart { Id = 1 };
            this.context.Carts.Add(cart);
            this.context.SaveChanges();
            var product = new Product { Name = "MugRoot" };
            var product2 = new Product { Name = "MugClassic" };
            this.context.Products.Add(product);
            this.context.SaveChanges();
            this.context.Products.Add(product2);
            this.context.SaveChanges();
            cart = this.context.Carts.FirstOrDefault(x => x.Id == 1);
            product = this.context.Products.FirstOrDefault(x => x.Name == "MugRoot");
            product2 = this.context.Products.FirstOrDefault(x => x.Name == "MugClassic");
            this.cartService.AddProductToCart(product, cart);
            this.cartService.AddProductToCart(product2, cart);
            Assert.That(this.context.CartProducts.Count(), Is.EqualTo(2));
            Assert.That(cart.Products.Count(), Is.EqualTo(2));
        }

        [Test]
        public void RemoveProductFromCartRemovesConnections()
        {
            var cart = new Cart { Id = 1 };
            this.context.Carts.Add(cart);
            this.context.SaveChanges();
            var product = new Product { Name = "MugRoot" };
            var product2 = new Product { Name = "MugClassic" };
            this.context.Products.Add(product);
            this.context.SaveChanges();
            this.context.Products.Add(product2);
            this.context.SaveChanges();
            cart = this.context.Carts.FirstOrDefault(x => x.Id == 1);
            product = this.context.Products.FirstOrDefault(x => x.Name == "MugRoot");
            product2 = this.context.Products.FirstOrDefault(x => x.Name == "MugClassic");
            this.cartService.AddProductToCart(product, cart);
            this.cartService.AddProductToCart(product2, cart);
            this.cartService.RemoveProductFromCart(cart, product);
            Assert.That(cart.Products.Count(), Is.EqualTo(1));
            Assert.That(cart.Products.FirstOrDefault().ProductId, Is.EqualTo(2));
        }

        [Test]
        public void GetCartProductsReturnsAllProducts()
        {
            var cart = new Cart { Id = 1 };
            this.context.Carts.Add(cart);
            this.context.SaveChanges();
            var product = new Product { Name = "MugRoot" };
            var product2 = new Product { Name = "MugClassic" };
            this.context.Products.Add(product);
            this.context.SaveChanges();
            this.context.Products.Add(product2);
            this.context.SaveChanges();
            cart = this.context.Carts.FirstOrDefault(x => x.Id == 1);
            product = this.context.Products.FirstOrDefault(x => x.Name == "MugRoot");
            product2 = this.context.Products.FirstOrDefault(x => x.Name == "MugClassic");
            this.cartService.AddProductToCart(product, cart);
            this.cartService.AddProductToCart(product2, cart);
            var products=this.cartService.GetCartsProducts(cart).ToList();
            Assert.That(products.Count(), Is.EqualTo(2));
            Assert.That(products[0].Name, Is.EqualTo("MugRoot"));
            Assert.That(products[1].Name, Is.EqualTo("MugClassic"));
        }

        [Test]
        public void CalculateSumReturnsRightSum()
        {
            var cart = new Cart { Id = 1 };
            this.context.Carts.Add(cart);
            this.context.SaveChanges();
            var product = new Product { Name = "MugRoot", Price=20 };
            var product2 = new Product { Name = "MugClassic",Price=77 };
            this.context.Products.Add(product);
            this.context.SaveChanges();
            this.context.Products.Add(product2);
            this.context.SaveChanges();
            cart = this.context.Carts.FirstOrDefault(x => x.Id == 1);
            product = this.context.Products.FirstOrDefault(x => x.Name == "MugRoot");
            product2 = this.context.Products.FirstOrDefault(x => x.Name == "MugClassic");
            this.cartService.AddProductToCart(product, cart);
            this.cartService.AddProductToCart(product2, cart);
            decimal sum=this.cartService.CalculateProductsSum(cart);
            Assert.That(sum, Is.EqualTo(97));
        }

        [Test]
        public void CheckOutDeductsAndAdds()
        {
            var user = new User { UserName = "xXPimpXx",EzBucks=100 };
            var seller = new User { UserName = "CameraStore",EzBucks=20 };
            this.context.Users.Add(user);
            this.context.SaveChanges();
            this.context.Users.Add(seller);
            this.context.SaveChanges();

            user = this.context.Users.FirstOrDefault(x => x.UserName == "xXPimpXx");
            seller = this.context.Users.FirstOrDefault(x => x.UserName == "CameraStore");
            this.cartService.CreateUsersCart(user);
            var cart = this.context.Carts.FirstOrDefault(x => x.Id == 1);
            
            var product = new Product { Name = "MugRoot", Price = 20,UserId=seller.Id };
            var product2 = new Product { Name = "MugClassic", Price = 77,UserId=seller.Id };
            this.context.Products.Add(product);
            this.context.SaveChanges();
            this.context.Products.Add(product2);
            this.context.SaveChanges();

            cart = this.context.Carts.FirstOrDefault(x => x.Id == 1);
            product = this.context.Products.FirstOrDefault(x => x.Name == "MugRoot");
            product2 = this.context.Products.FirstOrDefault(x => x.Name == "MugClassic");
            this.cartService.AddProductToCart(product, cart);
            this.cartService.AddProductToCart(product2, cart);

            this.cartService.CheckOut(user, 0);
            Assert.That(user.EzBucks, Is.EqualTo(3));
            Assert.That(seller.EzBucks, Is.EqualTo(117));
        }
    }
}