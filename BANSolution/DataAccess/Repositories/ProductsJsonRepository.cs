using Domain.Interfaces;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class ProductsJsonRepository : IProduct
    {
        //GetProducts()
        //GetProduct()
        //AddProduct()
        //...

        private string _path;
        public ProductsJsonRepository(string path) {

            _path = path;

            if (System.IO.File.Exists(path) == false)
            {
                using (var myFile = System.IO.File.Create(path))
                {
                    myFile.Close();
                }
            }

        }

        public void AddProduct(Product product)
        {
            var list = GetProducts().ToList();
            list.Add(product);

            string myProductsJsonString = JsonSerializer.Serialize(list);


            System.IO.File.WriteAllText(_path, myProductsJsonString);

        }

        public void DeleteProduct(Guid id)
        {
            throw new NotImplementedException();
        }

        public Product? GetProduct(Guid id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Product> GetProducts()
        {
            string allProductsAsJson = "";
            using(var myFile = System.IO.File.OpenText(_path)) //if file doesn't exist, it will throw an exception
            {
                allProductsAsJson = myFile.ReadToEnd();
                myFile.Close();
            }

            if (string.IsNullOrEmpty(allProductsAsJson))
            {
                return new List<Product>().AsQueryable();
            }
            else
            {
                //Deserialization >>> transforming string to object; in our case converting from json string into a List<Product>
                List<Product> allProducts = JsonSerializer.Deserialize<List<Product>>(allProductsAsJson);
                return allProducts.AsQueryable();
            }

        }

        public void UpdateProduct(Product product)
        {
            throw new NotImplementedException();
        }
    }
}
