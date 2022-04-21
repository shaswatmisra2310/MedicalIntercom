using MedicalIntercomProject.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace MedicalIntercomProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            //UserDbContext userdbContext = new UserDbContext();

            //var role = new Role() { ID = 1, RoleName = "Admin" };
            //userdbContext.Roles.Add(role);
            //userdbContext.UsersTable.Add(new User() { Id = 1, FirstName = "abc", LastName = "xyz", RoleId = 1, password = "12345" });
            //userdbContext.SaveChanges();
        
            return View();
        }
        [Authorize]
        public IActionResult Loggedin()
        {
            //ViewBag.CurrentUser = HttpContext.User.Claims.First(c => c.Type == ClaimTypes.Name).Value;
            //var CurrentUser = HttpContext.User.Claims.First(c => c.Type == ClaimTypes.Name).Value;
            //TempData["currentusername"]=CurrentUser;
            if (HttpContext.User.Claims.First(c => c.Type == ClaimTypes.Role).Value == "Admin")
                return RedirectToAction("Index", "Admin");
            //RedirectToAction("Index", "Setup");
            return View();
        }

        public IActionResult Loggedinuser()
        {
            return View();
        }

    }
}