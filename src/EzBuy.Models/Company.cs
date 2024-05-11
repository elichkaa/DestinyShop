namespace EzBuy.Models
{
    public class Company : EntityName
    {
        public string Description { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
