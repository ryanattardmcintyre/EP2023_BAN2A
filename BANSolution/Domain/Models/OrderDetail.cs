using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class OrderDetail
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
       
        public Product Product { get; set; }
        [ForeignKey("Product")]
        public int ProductFK { get; set; }

        public double Quantity { get; set; }

        public double Price { get; set; }

        [ForeignKey("Order")]
        public int OrderFK { get; set; } //ForeignKey attribute has to be applied on the actual foreign key property NOT the navigational property
        public Order Order { get; set; }
    }
}
