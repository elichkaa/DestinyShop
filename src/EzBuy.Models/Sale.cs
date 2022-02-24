using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzBuy.Models
{
    public class Sale:EntityName
    {
        
        public string Description { get; set; }
        public bool Seasonal { get; set; }
        public bool Company { get; set; }
        public bool Categorial { get; set; }
        public ICollection<Product> Products { get; set; }// probably shoud be many to many
}
}
