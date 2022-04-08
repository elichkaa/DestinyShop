namespace EzBuy.ViewModels.Cart
{
    public class CartViewModel
    {
        public int Id { get; set; }

        public CartProductViewModel[] Products { get; set; }

        public string PayAmount { get; set; }
    }
}
