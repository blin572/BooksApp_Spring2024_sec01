using BooksApp_Spring2024_sec01.Data;
using BooksApp_Spring2024_sec01.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BooksApp_Spring2024_sec01.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private BooksDbContext _dbContext;

        public CategoryController(BooksDbContext bookDbContext)
        {
            _dbContext = bookDbContext; //assigns context object to the provate instance variable
        }

        public IActionResult Index()
        {
            var listOfCategories = _dbContext.Categories.ToList();

            return View(listOfCategories);
        }

        public IActionResult Create()
        {
            BooksDbContext bookDbContext = _dbContext;
            return View();
        }
        /*
        public IActionResult Create(CategoryController categoryObj)
        {
            //test to make sure the category name is not 'test'
            if(categoryObj.Name.ToLower() == "test")
            {
                ModelState.AddModelError("name", "Category name cannot be 'test'")
            }

            //validation to make sure that category name and descriptio are not exactly the same
            if (categoryObj.Name.ToLower() == categoryObj.Description.ToLower())
            {
                ModelState.AddModelError("description", "Category name and category description cannot be the same")
            }

            if (ModelState.IsValid)
            {
                _dbContext.Categories.Add(categoryObj);
                _dbContext.SaveChanges();

                return RedirectToAction("Index");
            }
            return View();
        }
        */
        /*
        [HttpGet]
        public IActionResult Delete(int id)
        {
            Category categoryObj = _dbContext.Categories.Find(id);
            return View(categoryObj);

        }

        [HttpPost]
        [ActionName("Delete")]
        public IActionResult DeletePost(int id)
        {
            Category categoryObj = _dbContext.Categories.Find(id);

            if(categoyObj != null)
            {
                _dbContext.Categories.Remove(categoryObj);
                _dbContext.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(categoryObj);

        }
        */
        public IActionResult Details(int id)
        {
            Category categoryObj = _dbContext.Categories.Find(id);

            return View(categoryObj);
        }
    }

}
