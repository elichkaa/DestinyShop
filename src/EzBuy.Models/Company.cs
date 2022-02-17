using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzBuy.Models
{
    public class Company : MainEntity
    {
        public string Name { get; set; }
        public string Description { get; set; } 
        public ICollection<Product> Products { get; set; }
    }
}
