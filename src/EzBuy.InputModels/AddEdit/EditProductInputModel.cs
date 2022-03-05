using Microsoft.AspNetCore.Http;
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
        public int Id { get; set; }

        [Display(Name = "New product name")]
        public string? Name { get; set; }

        [Display(Name = "New product description")]
        public string? Description { get; set; }

        [Display(Name = "New product price")]
        [Range(1, double.MaxValue, ErrorMessage = "Price must be a positive number.")]
        public decimal Price { get; set; }

        [Display(Name = "Tags you wish to remove")]
        public string? RemoveTags { get; set; }

        [Display(Name = "Tags you wish to add")]
        public string? NewTags { get; set; }

        [Display(Name = "New product category")]
        public int Category { get; set; }

        [Display(Name = "Change product cover; The old one will be deleted.")]
        [DataType(DataType.Upload)]
        public IFormFile? Cover { get; set; }

        [Display(Name = "Remove product images")]
        [DataType(DataType.Upload)]
        public ICollection<IFormFile>? Images { get; set; }
    }
}
