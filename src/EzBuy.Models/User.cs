﻿using Microsoft.AspNetCore.Identity;

namespace EzBuy.Models
{
    public class User : IdentityUser<string>
    {
        public User()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Roles = new List<IdentityUserRole<string>>();
            this.Logins = new List<IdentityUserLogin<string>>();
        }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public virtual ICollection<IdentityUserRole<string>> Roles { get; set; }

        public virtual ICollection<IdentityUserLogin<string>> Logins { get; set; }

        public virtual ICollection<Product> Products { get; set; }

        public int? CartId { get; set; }

        public Cart Cart { get; set; }
        public decimal EzBucks { get; set; }
    }
}
