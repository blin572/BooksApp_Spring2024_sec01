using BooksApp_Spring2024_sec01.Data;
using BooksApp_Spring2024_sec01.Models;
using BooksApp_Spring2024_sec01.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe;
using Stripe.Checkout;
using System.Security.Claims;

namespace BooksApp_Spring2024_sec01.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize(Roles = "Customer")]
    public class CartController : Controller
    {
        private BooksDbContext _dbContext;

        public CartController(BooksDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var cartItemsList = _dbContext.Carts.Where(c => c.UserId == userId).Include(c => c.Books);

            ShoppingCartVM shoppingCartVM = new ShoppingCartVM
            {
                CartItems = cartItemsList,
                Order = new Order()
            };

            foreach (var cartItem in shoppingCartVM.CartItems)
            {
                cartItem.SubTotal = cartItem.Books.Price * cartItem.Quantity;

                shoppingCartVM.Order.OrderTotal += cartItem.SubTotal;
            }

            return View(shoppingCartVM);
        }

        public IActionResult IncrementByOne(int id)
        {
            var cart = _dbContext.Carts.Find(id);
            cart.Quantity += 1;

            _dbContext.Update(cart);
            _dbContext.SaveChanges();

            return RedirectToAction("Index"); 
        }

        public IActionResult DecrementByOne(int id)
        {
            var cart = _dbContext.Carts.Find(id);

            if (cart.Quantity < 1)
            {
                _dbContext.Carts.Remove(cart);
                _dbContext.SaveChanges();
            }
            else
            {
                cart.Quantity -= 1;
                _dbContext.Update(cart);
                _dbContext.SaveChanges();
            }          

            return RedirectToAction("Index");
        }

        public IActionResult RemoveFromCart(int id)
        {
            var cart = _dbContext.Carts.Find(id);
            _dbContext.Carts.Remove(cart);
            _dbContext.SaveChanges();

            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult ReviewOrder()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cartItemsList = _dbContext.Carts.Where(c => c.UserId == userId).Include(c => c.Books);

            ShoppingCartVM shoppingCartVM = new ShoppingCartVM
            {
                CartItems = cartItemsList,
                Order = new Order()
            };

            foreach (var cartItem in shoppingCartVM.CartItems)
            {
                cartItem.SubTotal = cartItem.Books.Price * cartItem.Quantity;

                shoppingCartVM.Order.OrderTotal += cartItem.SubTotal;
            }

            shoppingCartVM.Order.ApplicationUser = _dbContext.ApplicationUsers.Find(userId);
            shoppingCartVM.Order.Name = shoppingCartVM.Order.ApplicationUser.Name;
            shoppingCartVM.Order.StreetAddress = shoppingCartVM.Order.ApplicationUser.StreetAddress;
            shoppingCartVM.Order.City = shoppingCartVM.Order.ApplicationUser.City;
            shoppingCartVM.Order.State = shoppingCartVM.Order.ApplicationUser.State;
            shoppingCartVM.Order.PostalCode = shoppingCartVM.Order.ApplicationUser.PostalCode;
            shoppingCartVM.Order.PhoneNumber = shoppingCartVM.Order.ApplicationUser.PhoneNumber;

            return View(shoppingCartVM);
        }

        [HttpPost]
        [ActionName("ReviewOrder")]
        public IActionResult ReviewOrderPOST(ShoppingCartVM shoppingCartVM)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cartItemsList = _dbContext.Carts.Where(c => c.UserId == userId).Include(c => c.Books);
            shoppingCartVM.CartItems = cartItemsList;

            foreach (var cartItem in shoppingCartVM.CartItems)
            {
                cartItem.SubTotal = cartItem.Books.Price * cartItem.Quantity;

                shoppingCartVM.Order.OrderTotal += cartItem.SubTotal;
            }

            shoppingCartVM.Order.ApplicationUser = _dbContext.ApplicationUsers.Find(userId);
            shoppingCartVM.Order.Name = shoppingCartVM.Order.ApplicationUser.Name;
            shoppingCartVM.Order.StreetAddress = shoppingCartVM.Order.ApplicationUser.StreetAddress;
            shoppingCartVM.Order.City = shoppingCartVM.Order.ApplicationUser.City;
            shoppingCartVM.Order.State = shoppingCartVM.Order.ApplicationUser.State;
            shoppingCartVM.Order.PostalCode = shoppingCartVM.Order.ApplicationUser.PostalCode;
            shoppingCartVM.Order.PhoneNumber = shoppingCartVM.Order.PhoneNumber;
            shoppingCartVM.Order.OrderDate = DateOnly.FromDateTime(DateTime.Now);
            shoppingCartVM.Order.OrderStatus = "Pending";
            shoppingCartVM.Order.PaymentStatus = "Pending";

            _dbContext.Orders.Add(shoppingCartVM.Order);
            _dbContext.SaveChanges();

            foreach(var cartItem in shoppingCartVM.CartItems)
            {
                OrderDetail orderDetail = new()
                {
                    OrderId = shoppingCartVM.Order.OrderId,
                    BookID = cartItem.BookID,
                    Quantity = cartItem.Quantity,
                    Price = cartItem.Books.Price
                };
                _dbContext.OrderDetails.Add(orderDetail);
            }

            _dbContext.SaveChanges();

            if (!string.IsNullOrEmpty(shoppingCartVM.Order.ApplicationUser.Id))
            {


                //StripeConfiguration.ApiKey = "sk_test_51P6uo9IXln84tW8pPtpuHAty9qO7ovkgx50dRMSKbvr1f5JWhW1cNGJLCSkyapdD0gLm7LOsfkBteY7URGSZugZN00guoyWAG6";

                var options = new Stripe.Checkout.SessionCreateOptions
                {
                    SuccessUrl = "https://localhost:7286/" + $"customer/cart/OrderConfirmation?id={shoppingCartVM.Order.OrderId}",

                    CancelUrl = "https://localhost:7286/" + "customer/cart/index",

                    LineItems = new List<Stripe.Checkout.SessionLineItemOptions>(),
                    //{
                    //    new Stripe.Checkout.SessionLineItemOptions
                    //    {
                    //        Price = "price_1MotwRLkdIwHu7ixYcPLm5uZ",
                    //        Quantity = 2,
                    //    },
                    //},
                    Mode = "payment",
                };

                foreach (var cartItem in shoppingCartVM.CartItems)
                {
                    var sessionLineItem = new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = (long)cartItem.Books.Price,
                            Currency = "usd",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = cartItem.Books.Title
                            }
                        },
                        Quantity = cartItem.Quantity
                    };
                    options.LineItems.Add(sessionLineItem);
                }

                var service = new Stripe.Checkout.SessionService();
                Session session = service.Create(options);

                shoppingCartVM.Order.SessionID = session.Id;
                _dbContext.SaveChanges();

                Response.Headers.Add("Location", session.Url);
                return new StatusCodeResult(303);
            }
            
            return RedirectToAction("OrderConfirmation", new {id = shoppingCartVM.Order.OrderId});
        }

        //public void UpdatePaymentStatus(int orderID, string sessionID, string paymentIntentID)
        //{
        //    Order order = _dbContext.Orders.Find(orderID);

        //    if (!string.IsNullOrEmpty(sessionID))
        //    {
        //        order.SessionID = sessionID;
        //    }
        //    if(!string.IsNullOrEmpty(paymentIntentID))
        //    {
        //        order.PaymentIntentID = paymentIntentID;
        //        order.PaymentStatus = "Approved";
        //    }
        //}

        public IActionResult OrderConfirmation(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            Order order = _dbContext.Orders.Find(id);

            var sessID = order.SessionID;

            var service = new SessionService();
            Session session = service.Get(sessID);

            if(session.PaymentStatus.ToLower() == "paid")
            {
                order.PaymentIntentID = session.PaymentIntentId;

                order.PaymentStatus = "Approved";
            }

            List<Cart> userCartItems = _dbContext.Carts.ToList().Where(u => u.UserId == userId).ToList();
           
            _dbContext.Carts.RemoveRange(userCartItems);
            _dbContext.SaveChanges();

            return View(id);
        }

       
    }
}
