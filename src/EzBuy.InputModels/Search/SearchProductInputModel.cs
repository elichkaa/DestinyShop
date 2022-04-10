using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        [Display(Name = "Category")]
        public string Category { get; set; }
    }
}
