using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzBuy.Models
{
    public class Category : EntityName
    {
        public ICollection<Product> Products { get; set; }
    }
}
