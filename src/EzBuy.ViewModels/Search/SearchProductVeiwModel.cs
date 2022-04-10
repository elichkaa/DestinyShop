using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzBuy.ViewModels.Search
{
    public class SearchProductViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string SellerName { get; set; }

        public string ReleaseDate { get; set; }
        public string Category { get; set; }

    }
}
