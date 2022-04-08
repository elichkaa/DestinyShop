using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzBuy.Services.Contracts
{
    public interface IAddEzBucks
    {
        public void AddEzBucksToUser(string userEmail, string packageName);
    }
}
