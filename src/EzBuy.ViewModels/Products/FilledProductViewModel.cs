using EzBuy.Models;
using System.ComponentModel.DataAnnotations;

namespace EzBuy.ViewModels.Products
{
    public class FilledProductViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Product name")]
        public string Name { get; set; }

        [Range(1, double.MaxValue, ErrorMessage = "Price must be a positive number.")]
        [Display(Name = "Product price")]
        public decimal Price { get; set; }

        [Display(Name = "Product description")]
        public string Description { get; set; }

        [Display(Name = "Product category")]
        public int Category { get; set; }

        [Display(Name = "Product cover")]
        public Image Cover { get; set; }

        [Display(Name = "Product images")]
        public ICollection<Image> Images { get; set; }

        [Display(Name = "Product manufacturer")]
        public string Manufacturer { get; set; }

        [Display(Name = "Current tags")]
        public string Tags { get; set; }
    }
}
