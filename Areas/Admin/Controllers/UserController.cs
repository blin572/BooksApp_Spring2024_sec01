using BooksApp_Spring2024_sec01.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BooksApp_Spring2024_sec01.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private BooksDbContext _dbContext;
        private UserManager<IdentityUser> _userManager; 

        public UserController(BooksDbContext dbContext, UserManager<IdentityUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            List<ApplicationUser> userList = _dbContext.ApplicationUsers.ToList();
            var allRoles = _dbContext.Roles.ToList();
            var userRoles = _dbContext.UserRoles.ToList();

            foreach(var user in userList)
            {
                var roleId = userRoles.Find(r => r.UserId == user.Id).RoleId;
                var roleName = allRoles.Find(r => r.Id == roleId).Name;
                user.RoleName = roleName;
            }
            return View(userList);
        }

        public IActionResult LockUnlock(string id)
        {
            var userFromDB = _dbContext.ApplicationUsers.Find(id);

            if(userFromDB.LockoutEnd != null && userFromDB.LockoutEnd > DateTime.Now)
            {
                userFromDB.LockoutEnd = DateTime.Now;
            }
            else
            {
                userFromDB.LockoutEnd = DateTime.Now.AddYears(10);
            }
            _dbContext.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult EditUserRole(string id)
        {
            var currentUserRole = _dbContext.UserRoles.FirstOrDefault(ur => ur.UserId == id);

            IEnumerable<SelectListItem> listOfRoles = _dbContext.Roles.ToList().Select(r => new SelectListItem
            {
                Text = r.Name,
                Value = r.Id.ToString()
            });

            ViewBag.ListOfRoles = listOfRoles;
            ViewBag.UserInfo = _dbContext.ApplicationUsers.Find(id);
            return View(currentUserRole);
        }

        [HttpPost]
        public IActionResult EditUserRole(Microsoft.AspNetCore.Identity.IdentityUserRole<string> updateRole)
        {
            ApplicationUser applicationUser = _dbContext.ApplicationUsers.Find(updateRole.UserId);
            string newRoleName = _dbContext.Roles.Find(updateRole.RoleId).Name;
            string oldRoleId = _dbContext.UserRoles.FirstOrDefault(u => u.UserId == applicationUser.Id).RoleId;
            string oldRoleName = _dbContext.Roles.Find(oldRoleId).Name;

            _userManager.RemoveFromRoleAsync(applicationUser, oldRoleName).GetAwaiter().GetResult();
            _userManager.AddToRoleAsync(applicationUser, newRoleName).GetAwaiter().GetResult();

            return RedirectToAction("Index");
        }
    }
}
