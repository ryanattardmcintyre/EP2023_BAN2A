using AspNetCore;
using DataAccess.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace PresentationWebApp.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated==false) {
                //give an error to the user
                //block the user

                TempData["error"] = "Access Denied";
                return RedirectToAction("Index", "Home");
            }
            return View();
        }



        //public IActionResult DiscountAProductPrice([FromServices] ProductsRepository pr, double rate)
        //{
        //    //pr.UpdateProduct(...);
            
        //}

        //public IActionResult DiscontinueAProduct([FromServices] ProductsRepository pr, Guid id)
        //{
        //    ProductsController productsController = new ProductsController();
        //    productsController.PR = pr;
        //    productsController.Delete(id);

        //}
    }
}
