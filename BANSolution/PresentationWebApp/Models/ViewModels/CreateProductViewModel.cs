using Domain.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using PresentationWebApp.Validators;
using DataAccess.Repositories;

namespace PresentationWebApp.Models.ViewModels
{
    public class CreateProductViewModel
    {
    

        public List<Category> Categories { get; set; } //for display purposes 

        [Required(ErrorMessage ="Name cannot be left blank")]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }

        [Required]
        [Range(0.000000001, double.MaxValue, ErrorMessage ="Price has to be a positive value")]
        public double Price { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int Stock { get; set; }

        [Required]
        [CategoryValidation()]
        public int CategoryFK { get; set; } //foreign key property: holds the value

        public string Supplier { get; set; }

        [DisplayName("Wholesale Price")]
        public double WholesalePrice { get; set; }

        public string? Image { get; set; }

        public IFormFile ImageFile { get; set; }
        



    }
}
