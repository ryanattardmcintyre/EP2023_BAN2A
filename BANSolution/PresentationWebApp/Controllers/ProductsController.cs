using DataAccess.Repositories;
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

        
    }
}
