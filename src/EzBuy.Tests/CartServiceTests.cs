using EzBuy.Data;
using EzBuy.Models;
using EzBuy.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

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
        [Fact]
        public void CreateUserCartCreatesCart()
        {
            var user = new User { UserName = "xXPimpXx" };
            this.context.Users.Add(user);
            this.context.SaveChanges();
            user = this.context.Users.FirstOrDefault(x => x.UserName == "xXPimpXx");
            this.cartService.CreateUsersCart(user);
            NUnit.Framework.Assert.AreEqual(1, this.context.Carts.Count());
            var cart = this.context.Carts.FirstOrDefault(x => x.Id == 1);
            NUnit.Framework.Assert.AreEqual("xXPimpXx", cart.User.UserName);
        }
        [Fact]
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
            NUnit.Framework.Assert.AreEqual(2, this.context.CartProducts.Count());
            NUnit.Framework.Assert.AreEqual(2, cart.Products.Count());
        }
        [Fact]
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
            NUnit.Framework.Assert.AreEqual(1, cart.Products.Count());
            NUnit.Framework.Assert.AreEqual(2, cart.Products.FirstOrDefault().ProductId);
        }
        [Fact]
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
            NUnit.Framework.Assert.AreEqual(2, products.Count());
            NUnit.Framework.Assert.AreEqual("MugRoot", products[0].Name);
            NUnit.Framework.Assert.AreEqual("MugClassic", products[1].Name);
        }
        [Fact]
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
            NUnit.Framework.Assert.AreEqual(97, sum);
        }
        [Fact]
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
            NUnit.Framework.Assert.AreEqual(3, user.EzBucks);
            NUnit.Framework.Assert.AreEqual(117, seller.EzBucks);
        }
    }
}