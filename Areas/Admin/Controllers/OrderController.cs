using BooksApp_Spring2024_sec01.Data;
using BooksApp_Spring2024_sec01.Models;
using BooksApp_Spring2024_sec01.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BooksApp_Spring2024_sec01.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin, Employee")]
    public class OrderController : Controller
    {
        private BooksDbContext _dbContext;
        [BindProperty]
        public OrderVM orderVM { get; set; }

        public OrderController(BooksDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IActionResult Index()
        {
            IEnumerable<Order> listOfOrders = _dbContext.Orders.Include(o => o.ApplicationUser);
            return View(listOfOrders);
        }

        public IActionResult Details(int id)
        {
            Order order = _dbContext.Orders.Find(id);
            _dbContext.Entry(order).Reference(o => o.ApplicationUser).Load();
            IEnumerable<OrderDetail> orderDetails = _dbContext.OrderDetails.Where(od => od.OrderId == id).Include(od => od.Books);

            OrderVM orderVm = new OrderVM
            {
                Order = order,
                OrderDetails = orderDetails
            };
            return View(orderVm);
        }
        [HttpPost]
        public IActionResult UpdateOrderInformation()
        {
            Order orderFromDB = _dbContext.Orders.Find(orderVM.Order.OrderId);
            orderFromDB.Name = orderVM.Order.Name;
            orderFromDB.StreetAddress = orderVM.Order.StreetAddress;
            orderFromDB.City = orderVM.Order.City;
            orderFromDB.State = orderVM.Order.State;
            orderFromDB.PostalCode = orderVM.Order.PostalCode;
            orderFromDB.PhoneNumber = orderVM.Order.PhoneNumber;
            orderFromDB.OrderStatus = orderVM.Order.OrderStatus;

            //if(!string.IsNullOrEmpty(orderVM.Order.Carrier))
                orderFromDB.Carrier = orderVM.Order.Carrier;
            //if(!string.IsNullOrEmpty(orderVM.Order.ShippingDate.ToString()))
                orderFromDB.ShippingDate = orderVM.Order.ShippingDate;
            //if(!string.IsNullOrEmpty(orderVM.Order.TrackingNumber))
                orderFromDB.TrackingNumber = orderVM.Order.TrackingNumber;

            _dbContext.Orders.Update(orderFromDB);
            _dbContext.SaveChanges();

            return RedirectToAction("Details", new {id = orderFromDB.OrderId});
        }

        public IActionResult ProcessOrder()
        {
            Order order = _dbContext.Orders.Find(orderVM.Order.OrderId);
            order.OrderStatus = "Processing";
            order.ShippingDate = DateOnly.FromDateTime(DateTime.Now).AddDays(7);
            order.Carrier = "USPS";

            _dbContext.Orders.Update(order);
            _dbContext.SaveChanges();

            return RedirectToAction("Details", new {id = order.OrderId});
        }

        public IActionResult CompleteOrder()
        {
            Order order = _dbContext.Orders.Find(orderVM.Order.OrderId);
            order.OrderStatus = "Shipped and Complete";
            order.ShippingDate = DateOnly.FromDateTime(DateTime.Now);

            _dbContext.Orders.Update(order);
            _dbContext.SaveChanges();

            return RedirectToAction("Details", new { id = order.OrderId });
        }
    }
}
