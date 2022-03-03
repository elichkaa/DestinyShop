using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzBuy.ViewModels.Products
{
    
    public class ProductOnAllPageViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string SellerName { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string Cover { get;set; }
        public string Category { get;set; }
        public int PageCount { get; set; }
        public int CurrentPage { get; set; }
    }
}
