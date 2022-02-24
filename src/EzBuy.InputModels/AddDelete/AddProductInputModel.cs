using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzBuy.InputModels.AddDelete
{
    public class AddProductInputModel
    {
        [Required]
        [Display(Name="Product name")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Product price")]
        public double Price { get; set; }

        [Required]
        [Display(Name = "Product description")]
        public string Description { get; set; }
        [Required]
        [Display(Name = "Product category")]
        public string Category { get; set; }

        //[Display(Name = "Product cover")]
        //[DataType(DataType.Upload)]
        //[Required]
        //public IFormFile Cover { get; set; }

        //And immages too
        
        [Display(Name = "Product manufacturer/ optional")]
        public string Manufacturer { get; set; }

        [Display(Name = "Tags/ optional")]
        public string Tags { get; set; }


    }
}
