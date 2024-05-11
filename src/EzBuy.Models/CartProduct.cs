namespace EzBuy.Models
{
    public class CartProduct
    {
        public int ProductId { get; set; }

        public Product Product { get; set; } = null!;

        public int CartId { get; set; }

        public Cart Cart { get; set; } = null!;
    }
}

