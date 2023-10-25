using DataAccess.DataContext;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    //note1: the role of repository classes will be fetch or write data from or to the database directly
    //       no need to code complex logic here
    //       just the 4 CRUD (creation, reading, updating, deleting) operations



    //note2: Dependency Injection
    //to make a call to use an instance: Constructor injection vs Method Injection
    /*
     * Checkout
     * 
     * 1. Check whether i have enough products in stock. ProductsRepository -> GetProduct(int id)
     * 2. Charge the client's visa. PaymentRepository -> ChargeVisa(int userid)
     * 3. Deduct the qty from the stock. ProductsRepository -> UpdateProduct(id, newstock)
     * 4. Place an Order in the Orders table. OrdersRepository -> CreateOrder()
     * 5. Place the subsequent OrderDetails in the OrderDetails table -> CreateOrderDetail(...)
     * ...
     * 
     * in each of the repository classes (3), you will make a different call to the same class to create different
     * objects ShoppingCartDbContext()......
     */
    public class ProductsRepository
    {
        private ShoppingCartDbContext _shoppingCartDbContext;
        public ProductsRepository(ShoppingCartDbContext shoppingCartDbContext ) {
            _shoppingCartDbContext= shoppingCartDbContext;
        }

        /*
         IQueryable vs List

        //1. (disadv) it doesn't allow you to debug what's inside it
        //2. (adv) it doesn't open a connection to the database until a ToList() is applied
        //3. (note) it prepares the statement that needs to be executed at some point

        // var myProducts = GetProducts().Where(x=>x.Name.Contains("Sam")).Skip(x).Take(y).OrderBy(x=>x.Name).ToList();
         
        //Select * From Products Where Product.Name like '%Sam%' order by Product.Name asc
         
         
         */

        public IQueryable<Product> GetProducts() { 
            return _shoppingCartDbContext.Products;
        }

        public Product? GetProduct(Guid id) {
            return _shoppingCartDbContext.Products.SingleOrDefault(x => x.Id == id);
        }

        public void AddProduct(Product product) {

            _shoppingCartDbContext.Products.Add(product);
            _shoppingCartDbContext.SaveChanges(); //leaving this out won't commit your changes
        
        } 
        public void UpdateProduct(Product product) { }

        public void DeleteProduct(Guid id) {
            var product = GetProduct(id);
            if (product != null)
            {
                _shoppingCartDbContext.Products.Remove(product);
                _shoppingCartDbContext.SaveChanges();
            }
            else throw new Exception("Product not found");
        }
    }
}
