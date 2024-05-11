namespace EzBuy.Models
{
    public class Image : MainEntity
    {
        public string Url { get; set; }
        public int? ProductId { get; set; }
        public Product Product { get; set; }
        public bool IsCover { get; set; }
    }
}
