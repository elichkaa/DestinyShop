using EzBuy.Data;
using EzBuy.Models;
using EzBuy.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzBuy.Services
{
    public class CartService:ICartService
    {
        private readonly EzBuyContext context;
        public CartService(EzBuyContext context)
        {
            this.context = context;
        }
        public void CreateUsersCart(User user)
        {
            var cart = new Cart{
                UserId = user.Id
            };
            this.context.Add(cart);
            this.context.SaveChanges();
        }
        public void AddProductToCart(Cart cart, Product product)
        {
            if (cart.Products.Count < 5)
            {
                context.CartProducts.Add(new CartProducts(product.Id, cart.Id));
            }
            else
            {
                throw new ArgumentException("You have too many items in your cart, check out first");
            }
        }
        public Cart GetCartByUserId(User user)
        {
            var cart=context.Carts.FirstOrDefault(x => x.UserId == user.Id);
            return cart;
        }
        public void RemoveProductFromCart(Cart cart, Product product)
        {
            CartProducts connection = (CartProducts)context.CartProducts.Where(x => x.ProductId == product.Id && x.CartId == cart.Id);
            context.CartProducts.Remove(connection);
            context.SaveChanges();
        }
        public ICollection<Product> GetCartsProducts(Cart cart)
        {
            var products=new List<Product>();
            foreach (var productConection in cart.Products)
            {
                products.Add(this.context.Products.FirstOrDefault(x => x.Id == productConection.ProductId));
            }
            if(products.Count > 0)
            {
                return products;
            }
            else
            {
                throw new ArgumentException("This cart is empty");
            }
        }
        public decimal CalculateProductsSum(Cart cart)
        {
            decimal sum = 0;
            var products = GetCartsProducts(cart);
            foreach(var product in products)
            {
                sum += product.Price;
            }
            return sum;
        }
        public void CheckOut(User user)
        {
            user.EzBucks -= CalculateProductsSum(user.Cart);
            this.context.Update(user);
            foreach(var product in GetCartsProducts(user.Cart))
            {
                RemoveProductFromCart(user.Cart, product);
            }
        }

        }
    }

