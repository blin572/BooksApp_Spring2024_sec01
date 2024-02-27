using BooksApp_Spring2024_sec01.Data;
using BooksApp_Spring2024_sec01.Models;
using BooksApp_Spring2024_sec01.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

namespace BooksApp_Spring2024_sec01.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BooksController : Controller
    {
        
        private BooksDbContext _dbContext;
        private IWebHostEnvironment _environment; //allows us to fetch info about the server
                                                  //on which the project is running
        public BooksController(BooksDbContext dbContext, IWebHostEnvironment environment)
        {
            _dbContext = dbContext;
            _environment = environment;
        }

        public IActionResult Index()
        {
            var listOfBooks = _dbContext.Books.ToList();

            return View(listOfBooks);
        }

        [HttpGet]
        public IActionResult Create()
        {
            IEnumerable<SelectListItem> listOfCategories = _dbContext.Categories.ToList().Select(o => new SelectListItem
            {
                Text = o.Name,
                Value = o.CategoryID.ToString(),
                //projects the category object into a SelectListItem object
            });

            // ViewBag.ListOfCategories = listOfCategories;
            // Same ^ 
            // ViewData["ListOfCategoriesVD"] = listOfCategories;

            //ViewModel with a book and all the categories as IEnumerable of SelectListItems 
            BookWIthCategoriesVM bookWithCategoriesVM = new BookWIthCategoriesVM();

            bookWithCategoriesVM.Book = new Books();
            bookWithCategoriesVM.ListOfCategories = listOfCategories;

            return View(bookWithCategoriesVM);
        }
        [HttpPost]
        public IActionResult Create(BookWIthCategoriesVM bookWIthCategoriesVM, IFormFile imgFile)
        {
            if (ModelState.IsValid)
            {
                string wwwrootPath = _environment.WebRootPath;
                if (imgFile != null)
                {
                    //save img file in the appropriate folder
                    using (var fileStream = new FileStream(Path.Combine(wwwrootPath,
                        @"images\bookImages\" + imgFile.FileName), FileMode.Create))
                    {
                        imgFile.CopyTo(fileStream); //saves the img file in the requested folder/path
                    }

                    bookWIthCategoriesVM.Book.ImgUrl = @"\images\bookImages\" + imgFile.FileName;
                }

                _dbContext.Books.Add(bookWIthCategoriesVM.Book);
                _dbContext.SaveChanges();

                return RedirectToAction("Index"); //goes back to list
            }
            return View(bookWIthCategoriesVM);//if the model isn't valid, the view displays the data that the user provided along with any validation errors
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var book = _dbContext.Books.Find(id);

            IEnumerable<SelectListItem> listOfCategories = _dbContext.Categories.ToList().Select(o => new SelectListItem
            {
                Text = o.Name,
                Value = o.CategoryID.ToString(),
            });

            BookWIthCategoriesVM bookWithCategoriesVM = new BookWIthCategoriesVM();
            bookWithCategoriesVM.Book = book;
            bookWithCategoriesVM.ListOfCategories = listOfCategories;

            return View(bookWithCategoriesVM);
        }

        [HttpPost]
        public IActionResult Edit(IFormFile? imgFile, BookWIthCategoriesVM bookWithCategoriesVM)
        {
            string wwwRootPath = _environment.WebRootPath;

            if (ModelState.IsValid)
            {
                if(imgFile != null)
                {
                    if (!string.IsNullOrEmpty(bookWithCategoriesVM.Book.ImgUrl))
                    {
                        //replace the file in the images folder
                        var oldImgPath = Path.Combine(wwwRootPath, bookWithCategoriesVM.Book.ImgUrl.TrimStart('\\'));
                        
                        if (System.IO.File.Exists(oldImgPath))
                        {
                            System.IO.File.Delete(oldImgPath);//deletes existing file
                        }

                        using (var fileStream = new FileStream(Path.Combine(wwwRootPath,
                        @"images\bookImages\" + imgFile.FileName), FileMode.Create))
                        {
                            imgFile.CopyTo(fileStream); //saves the img file in the requested folder/path
                        }
                        //replace the url in the database
                        bookWithCategoriesVM.Book.ImgUrl = @"\images\bookImages\" + imgFile.FileName;
                    }
                }

                _dbContext.Books.Update(bookWithCategoriesVM.Book);
                _dbContext.SaveChanges();

                return RedirectToAction("Index");
            }
            return View(bookWithCategoriesVM);
        }
    }
}
