namespace EzBuy.Models
{
    public class Product : EntityName
    {
        public decimal Price { get; set; }
        public string Description { get; set; }
        public int? CategoryId { get; set; }
        public Category Category { get; set; }
        public int? CompanyId { get; set; }
        public Company Company { get; set; }
        public ICollection<Image> Images { get; set; }
        public int? ManufacturerId { get; set; }
        public Manufacturer Manufacturer { get; set; }
        public ICollection<ProductTags> Tags { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        public ICollection<Cart> Carts { get; set; } = [];
        public ICollection<CartProduct> CartProducts { get; set; } = [];
        public int? SaleId { get; set; }
        public int? SaleOffPrecentage { get; set; }
        public Sale Sale { get; set; }// probably shoud be many to many
        public DateTime? DateListed { get; set; }
    }
}
