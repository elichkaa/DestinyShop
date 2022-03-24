using EzBuy.Data;
using EzBuy.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzBuy.Services
{
    public class PaymentService:IPaymentService
    {
        private readonly EzBuyContext context;
        public PaymentService()
        {
            context = new EzBuyContext();
        }
    }
}
