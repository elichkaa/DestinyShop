using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzBuy.Models
{
    public class ProductTags
    {
        public ProductTags()
        {

        }
        public ProductTags(int productId, int tagId)
        {
            ProductId = productId;
            TagId = tagId;
        }
        public int ProductId { get; set; }

        public Product Product { get; set; }

        public int TagId { get; set; }

        public Tag Tag { get; set; }
    }
}
