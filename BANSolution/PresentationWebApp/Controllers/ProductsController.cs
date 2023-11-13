using DataAccess.Repositories;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using PresentationWebApp.Models.ViewModels;

namespace PresentationWebApp.Controllers
{
    public class ProductsController : Controller
    {
        public ProductsRepository PR { get; set; }

        private IProduct _productsRepository;
        private CategoriesRepository _categoriesRepository;

        //note (at the moment/ obsolete): the ProductsController accepts an instance of ProductsRepository
        //>>>>>
        //note (from now onwards): the ProductsController accepts ANY instance of IProducts i.e. it accepts ProductsRepository, ProductsJsonRepository, ProductsNoSqlRepository
        public ProductsController(IProduct productsRepository, CategoriesRepository categoriesRepository) {
            _categoriesRepository = categoriesRepository;
            _productsRepository = productsRepository;
        }

        public IActionResult Index()
        {
            var list = _productsRepository.GetProducts().OrderBy(x=>x.Name).ToList(); //there will be ONE database call

            var fixForCategory = _categoriesRepository.GetCategories().ToList();

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
                             Category = fixForCategory.SingleOrDefault(x=>x.Id == p.CategoryFK).Name // p.Category.Name //using the navigational property
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
            myModel.Categories = _categoriesRepository.GetCategories().ToList();
            
            return View(myModel); 
        
        }

        //.....user inputs the details

        //2. runs secondly with the parameters populated with the data....it saves into the db
        [HttpPost]
        public IActionResult Create(CreateProductViewModel model, [FromServices] IWebHostEnvironment host) {

            //note: (benefit) we are using an existent instance of productsRepository and not creating a new one!
            try
            {

                //code which will handle file upload
                //1. save the phyiscal file
                string relativePath = "";
                if (model.ImageFile != null)
                {
                    //1a. generation of a UNIQUE filename for our image
                    string newFilename = Guid.NewGuid().ToString() + System.IO.Path.GetExtension(model.ImageFile.FileName);

                    //1b. absolute path (where to save the file) e.g. C:\Users\attar\source\repos\EP2023_BAN2A\BANSolution\PresentationWebApp\wwwroot\images\nameOfTheFile.jpg
                    //IWebHostEnvironment
                    //esacape characters \" \r \n \t ....
                    string absolutePath = host.WebRootPath + "\\images\\" + newFilename;

                    //1c. relative path (to save into the db) e.g. \images\nameOfTheFile.jpg
                    relativePath = "/images/" + newFilename;

                    //1d. save the actual file using the absolute path

                    try
                    {
                        using (FileStream fs = new FileStream(absolutePath, FileMode.OpenOrCreate))
                        {
                            model.ImageFile.CopyTo(fs);
                            fs.Flush();
                        } //closing this bracket will close the filestream. if you don't close the filestream, you might get an error telling you that the File is being used by another process
                    }
                    catch (Exception)
                    {
                        //log the error
                    }
                }

                //2. set the path to be stored in the database
                _productsRepository.AddProduct(new Product()
                {
                    CategoryFK = model.CategoryFK,
                    Name = model.Name,
                    Description = model.Description,
                    Price = model.Price,
                    WholesalePrice = model.WholesalePrice,
                    Stock = model.Stock,
                    Supplier = model.Supplier,
                    Image = relativePath
                });

                if (relativePath == "")
                {
                    TempData["message"] = "No Image was uploaded but product was saved successfully";
                }
                else TempData["message"] = "Product together with image was saved successfully";

                return RedirectToAction("Index");

            }
            catch (Exception ex)
            {
                TempData["error"] = "Product was not saved successfully";
                model.Categories = _categoriesRepository.GetCategories().ToList();
                return View(model);
            }
 
        }

        public IActionResult Details(Guid id)
        {
            var product = _productsRepository.GetProduct(id); //fetched the product from the db
            if (product == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                //
                ListProductsViewModel myProduct = new ListProductsViewModel()
                {
                    Category = product.Category.Name,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    Id = product.Id
                      ,
                    Stock = product.Stock
                      ,
                    Image = product.Image
                };

                return View(myProduct);
            }
        }

        public IActionResult Delete(Guid id)
        {
            try
            {
                var product= _productsRepository.GetProduct(id);
                if (product == null) TempData["error"] = "product was not found";
                else
                _productsRepository.DeleteProduct(id);
                
                TempData["message"] = "Product deleted successfully";
            }
            catch (Exception ex)
            {
                TempData["error"] = "Product was not deleted. Check the input";
            
            }

            return RedirectToAction("Index");

            //note: return View("Index") >>> is going to open directly the html page
            //note: return RedirectToAction("Index") >>> is going to trigger the action
        }


        //to load the page with textboxes where the user can type in the new details
        //overwriting the old details...therefore we need to show the user also the old details
        public IActionResult Edit(Guid id) {
        
           var originalProduct = _productsRepository.GetProduct(id);

            //to pass details to/from the pages/views we use viewmodels

            EditProductViewModel myModel = new EditProductViewModel();
            myModel.Categories = _categoriesRepository.GetCategories().ToList();


            myModel.Supplier = originalProduct.Supplier;
            myModel.WholesalePrice = originalProduct.WholesalePrice;
            myModel.Price = originalProduct.Price;
            myModel.Name = originalProduct.Name;
            myModel.CategoryFK = originalProduct.CategoryFK;
            myModel.Description = originalProduct.Description;
            myModel.Stock = originalProduct.Stock;
            myModel.Image = originalProduct.Image;
            myModel.Id = originalProduct.Id; // don't forget this!!! because when the View
                             // together with the edited is going to be resubmitted, 
                             //we need the id again to identify which
                             //product we have to update/overwrite

            return View(myModel);
        }

        [HttpPost]
        public IActionResult Edit(EditProductViewModel model, [FromServices] IWebHostEnvironment host)
        {
            //note: (benefit) we are using an existent instance of productsRepository and not creating a new one!
            try
            {
                //code which will handle file upload
                //1. save the phyiscal file
                string relativePath = "";
                string oldRelativePath =  _productsRepository.GetProduct(model.Id).Image; //images/1eac8e0b-8786-4692-8a7c-eb898bc0eb1d.jpg
                if (model.ImageFile != null) //the user decided to overwrite image
                {
                    //1a. generation of a UNIQUE filename for our image
                    string newFilename = Guid.NewGuid().ToString() + System.IO.Path.GetExtension(model.ImageFile.FileName);

                    //1b. absolute path (where to save the file) e.g. C:\Users\attar\source\repos\EP2023_BAN2A\BANSolution\PresentationWebApp\wwwroot\images\nameOfTheFile.jpg
                    //IWebHostEnvironment
                    //esacape characters \" \r \n \t ....
                    string absolutePath = host.WebRootPath + "\\images\\" + newFilename;

                    //1c. relative path (to save into the db) e.g. \images\nameOfTheFile.jpg
                    relativePath = "/images/" + newFilename;

                    //1d. save the actual file using the absolute path

                    try
                    {
                        using (FileStream fs = new FileStream(absolutePath, FileMode.OpenOrCreate))
                        {
                            model.ImageFile.CopyTo(fs);
                            fs.Flush();
                        } //closing this bracket will close the filestream. if you don't close the filestream, you might get an error telling you that the File is being used by another process

                        //after the new image is saved, we can delete the old one

                        //get the old path and delete
                        
                        var oldAbsolutePath = host.WebRootPath + "\\images\\" + System.IO.Path.GetFileName(oldRelativePath);

                        System.IO.File.Delete(oldAbsolutePath);

                    }
                    catch (Exception)
                    {
                        //log the error
                    }
                }
                else
                {
                    relativePath = oldRelativePath;
                }

                //2. set the path to be stored in the database
                _productsRepository.UpdateProduct(new Product()
                {
                    CategoryFK = model.CategoryFK,
                    Name = model.Name,
                    Description = model.Description,
                    Price = model.Price,
                    WholesalePrice = model.WholesalePrice,
                    Stock = model.Stock,
                    Supplier = model.Supplier,
                    Image = relativePath,
                    Id = model.Id //<<<<<<<<<< very important in the Update/edit context. it will help the code identify which product to edit
                });

                if (relativePath == "")
                {
                    TempData["message"] = "No Image was uploaded but product was updated successfully";
                }
                else TempData["message"] = "Product together with image was updated successfully";

                return RedirectToAction("Index");

            }
            catch (Exception ex)
            {
                TempData["error"] = "Product was not saved successfully";
                model.Categories = _categoriesRepository.GetCategories().ToList();
                return View(model);
            }
        }
    }
}
