using DataAccess.Repositories;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using PresentationWebApp.Models.ViewModels;

namespace PresentationWebApp.Controllers
{
    public class ProductsController : Controller
    {
        private ProductsRepository _productsRepository;
        public ProductsController(ProductsRepository productsRepository) {

            _productsRepository = productsRepository;
        }

        public IActionResult Index()
        {
            var list = _productsRepository.GetProducts().OrderBy(x=>x.Name).ToList(); //there will be ONE database call

            //transfer from Product >>>>> ProductViewModel
            var result = from p in list
                         select new ListProductsViewModel()
                         {
                             Name = p.Name,
                             Description = p.Description,
                             Id = p.Id,
                             Image = p.Image,
                             Price = p.Price,
                             Stock = p.Stock,
                             Category = p.Category.Name //using the navigational property
                         };


            return View(result);
        }

        public IActionResult Search(string keyword) {
            
            //Select * From Products

            var list = _productsRepository.GetProducts()
                .Where(x=>x.Name.StartsWith(keyword) || x.Description.Contains(keyword))
                .OrderBy(x => x.Name).ToList(); //there will be ONE database call

            //transfer from Product >>>>> ProductViewModel
            var result = from p in list
                         select new ListProductsViewModel()
                         {
                             Name = p.Name,
                             Description = p.Description,
                             Id = p.Id,
                             Image = p.Image,
                             Price = p.Price,
                             Stock = p.Stock,
                             Category = p.Category.Name //using the navigational property
                         };


            return View("Index",result); //opening the Index View but passing a filtered list

        }


        //1. runs first and loads the page with empty fields to the user
        [HttpGet]
        public IActionResult Create() {
            CreateProductViewModel myModel = new CreateProductViewModel();
            //populate the Categories list from the db

            
            return View(); 
        
        }

        //.....user inputs the details

        //2. runs secondly with the parameters populated with the data....it saves into the db
        [HttpPost]
        public IActionResult Create(Product p) { }
        
    }
}
