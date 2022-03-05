using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzBuy.InputModels.AddEdit
{
    public class AddCurencyInputModel
    {
        //dropdownmenu again
        [Required]
        [Display(Name = "Select your credit card issuer")]
        public string PaymentMethod { get; set; }
        [Required]
        [Display(Name = "Card Number")]
        public string CardNumber { get; set; }
        [Required]
        [Display(Name = "First name")]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "Last name")]
        public string LastName { get; set; }
        [Required]
        [Display(Name = "Billing adress")]
        public string BillingAdress { get; set; }
        
        [Display(Name = "Second billing adress(optional)")]
        public string BillingAdress2 { get; set; }

        [Required]
        [Display(Name = "Country")]
        public string Country { get; set; }

        [Required]
        [Display(Name = "City")]
        public string City { get; set; }

        [Required]
        [Display(Name = "Security code")]
        public string SecurityCode { get; set; }
    }
}
