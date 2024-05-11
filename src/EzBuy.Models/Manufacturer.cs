namespace EzBuy.Models
{
    public class Manufacturer : EntityName
    {
        public string Description { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
