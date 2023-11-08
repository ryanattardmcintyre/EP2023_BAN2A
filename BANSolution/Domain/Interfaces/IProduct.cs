using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    /// <summary>
    /// note: this interface will dictate what any (new) repository classes must have as methods
    /// </summary>
    public interface IProduct
    {
        IQueryable<Product> GetProducts();
        Product? GetProduct(Guid id);
        void AddProduct(Product product);
        void UpdateProduct(Product product);

        void DeleteProduct(Guid id);

    }
}
