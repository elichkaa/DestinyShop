using EzBuy.Data;
using EzBuy.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzBuy.Services
{
    public class AddEzBucks: IAddEzBucks
    {
        private readonly EzBuyContext context;
        public AddEzBucks(EzBuyContext context)
        {
            this.context = context;
        }
        public void AddEzBucksToUser(string userEmail, string packageName)
        {
            var user= this.context.Users.FirstOrDefault(x => x.Email == userEmail);
            if (packageName=="Small")
            {
                user.EzBucks += 50;
            }else if (packageName=="Medium")
            {
                user.EzBucks += 200;
            }
            else if(packageName=="Large"){
                user.EzBucks += 500;
            }
            context.Update(user);
            context.SaveChanges();
        }
    }
}
