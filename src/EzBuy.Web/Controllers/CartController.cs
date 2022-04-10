using EzBuy.Services.Contracts;
using EzBuy.ViewModels.Cart;
using Microsoft.AspNetCore.Mvc;
using EzBuy.Web.Controllers;

namespace EzBuy.Web.Controllers
{
    public class CartController : Controller
    {
        public const string SessionKeyName = "_Name";
        public const string SessionInitialPayment = "0";
        private readonly IProductService productService;

        public CartController(IProductService productService)
        {
            this.productService = productService;
        }

        public IActionResult Index()
        {
            var cart = SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "cart");
            ViewBag.cart = cart;
            if (cart == null || cart.Count == 0)
            {
                return View();
            }
            ViewBag.total = cart.Sum(item => item.Product.Price * item.Quantity);
            return View();

        }

        public IActionResult CartModal()
        {
            var products = SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "cart");
            return PartialView("_DisplayCart", products);
        }
        public async Task<IActionResult> DisplayCart()
        {
            return this.RedirectToAction("Index", "Package");
        }


        public async Task<IActionResult> Add(int id)
        {
            if (SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "cart") == null)
            {

                List<CartItem> cart = new List<CartItem>();
                var product = await this.productService.GetFilledProductById(id);
                var cartProduct = new CartProductViewModel()
                {
                    Id = product.Id,
                    ProductName = product.Name,
                    Price = product.Price,
                    ProductImage = product.Cover.Url
                };
                cart.Add(new CartItem { Product = cartProduct, Quantity = 1 });
                SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            }
            else
            {
                List<CartItem> cart = SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "cart");
                int index = doesExist(id.ToString(), cart);
                if (index != -1)
                {
                    cart[index].Quantity++;
                }
                else
                {
                    var product = await this.productService.GetFilledProductById(id);
                    var cartProduct = new CartProductViewModel()
                    {
                        Id = product.Id,
                        ProductName = product.Name,
                        Price = product.Price,
                        ProductImage = product.Cover.Url
                    };
                    cart.Add(new CartItem { Product = cartProduct, Quantity = 1 });
                }
                SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            }

            return RedirectToAction("Index");
        }


        public async Task<IActionResult> Remove(int id)
        {
            List<CartItem> cart = SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "cart");
            int index = doesExist(id.ToString(),cart);
            if(cart[index].Quantity > 1)
            {
                cart[index].Quantity--;
            }
            else
            {
                cart.RemoveAt(index);
            }
            SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Clear()
        {
            List<CartItem> cart = SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "cart");
            cart.Clear();
            SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            return RedirectToAction("Index");
        }

        private int doesExist(string id, List<CartItem> cart)
        {
            for (int i = 0; i < cart.Count; i++)
            {
                var existingId = cart[i].Product.Id.ToString();
                if (existingId.Equals(id))
                {
                    return i;
                }
            }
            return -1;
        }

    }
}
