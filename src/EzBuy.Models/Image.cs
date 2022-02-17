using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzBuy.Models
{
    public class Image : MainEntity
    {
        public string Url { get; set; }
        public int? ProductId { get; set; }
        public Product Product { get; set; }
    }
}
