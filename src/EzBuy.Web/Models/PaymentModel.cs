namespace EzBuy.Web.Models
{
    public class PaymentModel
    {
        public string PackageName { get; set; }
        public decimal Amount { get; set; }
        public string Company { get; set; }
        public string Description { get; set; }
        public string Label { get; set; }

    }
}
