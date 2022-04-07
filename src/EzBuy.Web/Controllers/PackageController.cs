using EzBuy.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace EzBuy.Web.Controllers
{
    public class PackageController : Controller
    {
        private static List<string> packages = new List<string>() { "Small", "Medium", "Large" };
        private static Dictionary<string, PaymentModel> models = new Dictionary<string, PaymentModel>();
        private readonly ChargeService chargeService;
        public PackageController(ChargeService chargeService)
        {
            this.chargeService = chargeService;
            if (models.Count == 0)
            {
                models.Add("Small", new PaymentModel
                {
                    PackageName = "Small",
                    Amount = 5M,
                    Company = "EzBuy",
                    Description = "Small EzBucks Package",
                    Label = "Pay 5$"
                });
                models.Add("Medium", new PaymentModel
                {
                    PackageName = "Medium",
                    Amount = 20M,
                    Company = "EzBuy",
                    Description = "Small EzBucks Package",
                    Label = "Pay 20$"
                });
                models.Add("Large", new PaymentModel
                {
                    PackageName = "Large",
                    Amount = 50M,
                    Company = "EzBuy",
                    Description = "Large EzBucks Package",
                    Label = "Pay 50$"
                });
            }
        }
        public IActionResult Index()
        {
            return View(packages);
        }
        [HttpGet("/Package/Order/{id}")]
        public IActionResult Order(string id)
        {
            
            return View(models[id]);
        }
        [HttpPost("/Package/Order/{id}")]
        public IActionResult Order(string id, string stripeToken, string stripeEmail)
        {
            Dictionary<string,string>Metadata=new Dictionary<string,string>();
            Metadata.Add("Product", id);
            Metadata.Add("Quantity", "1");
            var options = new ChargeCreateOptions
            {
                Amount = (long)(models[id].Amount * 100),
                Currency = "USD",
                Description = models[id].Description,
                Source = stripeToken,
                ReceiptEmail = stripeEmail,
                Metadata = Metadata
            };
            var charge=this.chargeService.Create(options);
            return Redirect("/");
        }
    }
}
