using BooksApp_Spring2024_sec01.Data;
using BooksApp_Spring2024_sec01.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace BooksApp_Spring2024_sec01.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, BooksDbContext bookDbContext)
        {
            _logger = logger;
            _dbContext = bookDbContext;
        }
        
        private BooksDbContext _dbContext;

        public IActionResult Index()
        {
            var booksList = _dbContext.Books.Include(b => b.category);
            return View(booksList.ToList());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
