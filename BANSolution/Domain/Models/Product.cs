using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Product
    {

        [Key] //reminder: set id to be auto-generated
        /*
         *  protected override void OnModelCreating(ModelBuilder modelBuilder)
                {
                    modelBuilder.Entity<YourEntity>()
                        .Property(e => e.Id)
                        .HasDefaultValueSql("NEWID()"); // This sets the default value to a new GUID

                    // Other entity configurations...

                    base.OnModelCreating(modelBuilder);
                }
         * 
         **/
        public Guid Id { get; set; }

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
        [ForeignKey("Category")]
        public int CategoryFK { get; set; } //foreign key property: holds the value

        public Category Category { get; set; } //navigational property: allows you to navigate through the Category properties

        public string Supplier { get; set; }
        public double WholesalePrice { get; set; }

        public string? Image { get; set; }
    }
}
