namespace EzBuy.Models
{
    public class Category : EntityName
    {
        public ICollection<Product> Products { get; set; }
    }
}
