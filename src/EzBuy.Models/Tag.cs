namespace EzBuy.Models
{
    public class Tag : EntityName
    {
        public ICollection<ProductTags> Products { get; set; }
    }
}
