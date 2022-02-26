using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzBuy.InputModels.AddEdit
{
    public class EditProductInputModel
    {
        [Display(Name = "New product name")]
        public string NewName { get; set; }

        [Display(Name = "New product description")]
        public string NewDescription { get; set; }

        [Display(Name = "New product price")]
        public decimal NewPrice { get; set; }

        [Display(Name = "Tags you wish to remove")]
        public string RemoveTags { get; set; }

        [Display(Name = "Tags you wish to add")]
        public string NewTags { get; set; }
    }
}
