using Domain.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PresentationWebApp.Models.ViewModels
{
    public class ListProductsViewModel
    {
        public Guid Id { get; set; }

        public string OwnerEmail { get; set; }

        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public double Price { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int Stock { get; set; }

        public string Category { get; set; } 

        public string? Image { get; set; }
    }
}
