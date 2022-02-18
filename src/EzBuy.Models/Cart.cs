namespace EzBuy.Models
{
    public class Cart : MainEntity
    {
        public string UserId { get; set; }
        public User User { get; set; }
        public ICollection<CartProducts> Products { get; set; }
    }
}