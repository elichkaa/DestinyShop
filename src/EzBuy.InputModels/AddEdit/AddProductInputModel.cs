using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzBuy.InputModels.AddEdit
{
    public class AddProductInputModel
    {
        [Required]
        [Display(Name="Product name")]
        public string Name { get; set; }

        [Required]
        [Range(1, double.MaxValue, ErrorMessage = "Price must be a positive number.")]
        [Display(Name = "Product price")]
        public decimal Price { get; set; }

        [Required]
        [Display(Name = "Product description")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Product category")]
        public int Category { get; set; }

        [Display(Name = "Product cover")]
        [DataType(DataType.Upload)]
        [Required]
        public IFormFile Cover { get; set; }

        [Display(Name = "Product images")]
        [DataType(DataType.Upload)]
        [Required]
        public ICollection<IFormFile> Images { get; set; }

        [Display(Name = "Product manufacturer")]
        [Required]
        public string Manufacturer { get; set; }

        [Display(Name = "Tags")]
        public string Tags { get; set; }
    }
}
