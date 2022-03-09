using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MedicalIntercomProject.Models;

namespace MedicalIntercomProject.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            RedirectToAction("LoggedIn", "Login");
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]

        public IActionResult NewUser()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateNewUser( NewUserViewModel newuser)
        {

            return View("Created","Admin");

        }
    }
}
