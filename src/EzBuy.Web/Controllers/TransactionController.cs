using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace EzBuy.Web.Controllers
{
    public class TransactionController : Controller
    {
        private readonly ChargeService chargeService;
        public TransactionController(ChargeService chargeService)
        {
            this.chargeService = chargeService;
        }
        public IActionResult Index()
        {
            return View(this.chargeService.List().ToList());
        }

        public IActionResult DeductMoney()
        {

            return this.RedirectToAction("Index");
        }
    }
}
