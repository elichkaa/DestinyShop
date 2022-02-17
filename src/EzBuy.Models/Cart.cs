namespace EzBuy.Models
{
    public class Cart : MainEntity
    {
        public int? UserId { get; set; }
        public User User { get; set; }
        public ICollection<CartProducts> Products { get; set; }
    }
}