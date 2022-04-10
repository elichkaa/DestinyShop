using EzBuy.Models;
using EzBuy.Services.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Issuing;

namespace EzBuy.Web.Controllers
{
    public class TransactionController : Controller
    {
        private readonly ChargeService chargeService;
        private readonly UserManager<User> userManager;
        private readonly ICartService cartService;

        public TransactionController(ChargeService chargeService, 
            UserManager<User> userManager, 
            ICartService cartService)
        {
            this.chargeService = chargeService;
            this.userManager = userManager;
            this.cartService = cartService;
        }
        public IActionResult Index()
        {
            return View(this.chargeService.List().ToList());
        }

        public async Task<IActionResult> DeductMoney(decimal amount)
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);
            await this.cartService.CheckOut(currentUser, amount);
            return this.RedirectToAction("Index");
        }
    }
}
