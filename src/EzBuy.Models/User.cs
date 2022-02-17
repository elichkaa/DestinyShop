using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzBuy.Models
{
    public class User : MainEntity
    {
        public string Name { get; set; }
        public string Password { get; set; }
        public ICollection<Product> Products { get; set; }
        public int? CartId { get; set; }
        public Cart Cart { get; set; }
    }
}
