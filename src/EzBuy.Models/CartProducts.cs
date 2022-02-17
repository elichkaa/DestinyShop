using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzBuy.Models
{
    public class CartProducts
    {
        public CartProducts(int productId, int cartId)
        {
            ProductId = productId;
            CartId = cartId;
        }
        public int ProductId { get; set; }

        public Product Product { get; set; }

        public int CartId { get; set; }

        public Cart Cart { get; set; }
    }
}

