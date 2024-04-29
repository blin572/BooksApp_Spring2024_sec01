using BooksApp_Spring2024_sec01.Data;
using BooksApp_Spring2024_sec01.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;

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
        [HttpGet]
        public IActionResult Details(int id)
        {
            Books book =_dbContext.Books.Find(id);

            _dbContext.Entry(book).Reference(b => b.category).Load();

            var cart = new Cart
            {
                BookID = id,
                Books = book,

                Quantity = 1
            };

            return View(cart);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Details(Cart cart)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            cart.UserId = userId; //plugged in userId into cart obj

            Cart existingCart = _dbContext.Carts.FirstOrDefault(c => c.UserId == userId && c.BookID == cart.BookID);

            if (existingCart != null) //if cart exists
            {
                //update existing row
                existingCart.Quantity += cart.Quantity;
                _dbContext.Carts.Update(existingCart);
            }
            else
            {
                _dbContext.Carts.Add(cart); //adding a new record into the cart Dbset
            }


            _dbContext.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
