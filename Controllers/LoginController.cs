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

        UserDbContext context;
       
        public LoginController(ILogger<LoginController> logger)
        {
            _logger = logger;
            context = new UserDbContext();
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

            User user = context.UsersTable.Where(x=>x.emailId == loginViewModel.username).FirstOrDefault(); 
            if (user == null)
                return View("check username");
            else
            {
                if ( loginViewModel.password == user.password)
                {
                    claims.Add(new Claim(ClaimTypes.Email, user.emailId));//username or email from name
                    var role = user.RoleId;
                    if (role == 1)
                        claims.Add(new Claim(ClaimTypes.Role, "Admin"));
                    else
                        claims.Add(new Claim(ClaimTypes.Role, "User"));
                
                }
                
                else
                {
                    ViewBag.IsUnauthorized = true;
                    return View(nameof(Index));
                }
                String currentUser=new String("currentUser");
                HttpContext.Session.SetString(currentUser, loginViewModel.username);
            }
            // Set Session values during button click  

            
                
                
            

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
