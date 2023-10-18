using Domain.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PresentationWebApp.Models.ViewModels
{
    public class CreateProductViewModel
    {
        public List<Category> Categories { get; set; } //for display purposes 

        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public double Price { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int Stock { get; set; }

        [Required]
        public int CategoryFK { get; set; } //foreign key property: holds the value

        public string Supplier { get; set; }
        public double WholesalePrice { get; set; }

        public string? Image { get; set; }
    }
}
