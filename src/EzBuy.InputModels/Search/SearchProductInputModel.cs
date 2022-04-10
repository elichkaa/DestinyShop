using System.ComponentModel.DataAnnotations;

namespace EzBuy.InputModels.Search
{
    public class SearchProductInputModel
    {
        [Required]
        [StringLength(30, MinimumLength = 2)]
        [Display(Name = "Product name")]
        public string Name { get; set; }

        [Display(Name = "Seller Name")]
        public string SellerName { get; set; }
    }
}
