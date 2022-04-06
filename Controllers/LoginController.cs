using MedicalIntercomProject.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;


namespace MedicalIntercomProject.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILogger<LoginController> _logger;

        public LoginController(ILogger<LoginController> logger)
        {
            _logger = logger;
        }
        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("Index");

            var claims = new List<Claim>();

            if (loginViewModel.username == "admin@mail.com" && loginViewModel.password == "123")
            {
                claims.Add(new Claim(ClaimTypes.Name, "admin@mail.com"));
                claims.Add(new Claim(ClaimTypes.Role, "Admin"));
            }
            else if (loginViewModel.username == "user" && loginViewModel.password == "user")
            {
                claims.Add(new Claim(ClaimTypes.Name, "user"));
                claims.Add(new Claim(ClaimTypes.Role, "User"));
            }
            else
            {
                ViewBag.IsUnauthorized = true;
                return View(nameof(Index));
            }

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
            return RedirectToAction("LoggedIn","Home");
        }


        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index","Home");
        }

        [AllowAnonymous]

        public IActionResult UnauthorizedUser()
        {
            return View();
        }
    }
}
