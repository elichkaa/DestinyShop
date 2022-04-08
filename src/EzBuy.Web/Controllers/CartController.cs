using EzBuy.ViewModels.Cart;
using Microsoft.AspNetCore.Mvc;

namespace EzBuy.Web.Controllers
{
    public class CartController : Controller
    {
        public const string SessionKeyName = "_Name";
        public const string SessionInitialPayment = "0";
        public CartController()
        {
            
        }

        public IActionResult CartModal()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(SessionKeyName)))
            {
                HttpContext.Session.SetString(SessionKeyName, "Cart");
                HttpContext.Session.SetString(SessionInitialPayment, "0");
            }
            return PartialView("_DisplayCart");
        }
        public async Task<IActionResult> DisplayCart()
        {
            return this.Redirect("/");
        }


        public async Task AddToCart()
        {

        }

        public async Task RemoveFromCart()
        {

        }

        public async Task ClearCart()
        {

        }

        public async Task CheckOut()
        {

        }
    }
}
