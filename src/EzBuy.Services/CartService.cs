using EzBuy.Data;
using EzBuy.Models;
using EzBuy.Services.Contracts;

namespace EzBuy.Services
{
    public class CartService : ICartService
    {
        private readonly EzBuyContext context;
        public CartService(EzBuyContext context)
        {
            this.context = context;
        }
        public void CreateUsersCart(User user)
        {
            var cart = new Cart
            {
                UserId = user.Id
            };
            this.context.Carts.Add(cart);
            this.context.SaveChanges();
        }
        public void AddProductToCart(Product product, Cart cart)
        {
            int count = 0;
            if (cart.Products != null) { count = cart.Products.Count; }

            if (cart != null && cart.Products != null && count < 5)
            {
                cart.Products.Add(product);
                context.SaveChanges();
            }
            else
            {
                throw new ArgumentException("You have too many items in your cart, check out first");
            }
        }
        public Cart GetCartByUserId(User user)
        {
            var cart = context.Carts.FirstOrDefault(x => x.UserId == user.Id);
            if (cart == null) throw new ArgumentException("No such user or he doesnt have a cart");
            return cart;
        }
        public void RemoveProductFromCart(Cart cart, Product product)
        {
            foreach (var connection in cart.CartProducts)
            {
                if (connection.ProductId == product.Id)
                    context.CartProducts.Remove(connection);
                context.SaveChanges();
            }
        }
        public ICollection<Product> GetCartsProducts(Cart cart)
        {
            var products = new List<Product>();
            foreach (var productConection in cart.CartProducts)
            {
                products.Add(this.context.Products.FirstOrDefault(x => x.Id == productConection.ProductId));
            }
            if (products.Count > 0)
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
            foreach (var product in products)
            {
                sum += product.Price;
            }
            return sum;
        }
        public async Task CheckOut(User user, decimal amount)
        {
            user.EzBucks -= amount;
            this.context.Update(user);
            await this.context.SaveChangesAsync();
        }
    }
}

