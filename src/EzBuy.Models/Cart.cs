namespace EzBuy.Models
{
    public class Cart : MainEntity
    {
        public Guid UserId { get; set; }
        public User User { get; set; }
        public ICollection<Product> Products { get; set; } = [];
        public ICollection<CartProduct> CartProducts { get; set; } = [];
    }
}