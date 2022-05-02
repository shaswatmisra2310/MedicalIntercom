using MedicalIntercomProject.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace MedicalIntercomProject.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILogger<LoginController> _logger;

        UserDbContext context;
        const string SessionName = "_Name";
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

            SHA256 SHA256instance = SHA256.Create();

            var claims = new List<Claim>();

            User user = context.UsersTable.Where(x=>x.emailId == loginViewModel.EmailID).FirstOrDefault(); 
            if (user == null)
            {
                ViewBag.IsUnauthorized = true;
                return View(nameof(Index));
            }
            else
            {
                //byte[] bytes = Encoding.UTF8.GetBytes(loginViewModel.password);
                //var passwordcheck = SHA256instance.ComputeHash(bytes);
                //var stringpassword = String.Join("", passwordcheck);
                if (loginViewModel.password == user.password)
                {
                    claims.Add(new Claim(ClaimTypes.Email, user.emailId));
                    var role = user.RoleId;
                    if (role == 1)
                        claims.Add(new Claim(ClaimTypes.Role, "Admin"));
                    else
                        claims.Add(new Claim(ClaimTypes.Role, "User"));
                    
                    var logintime = DateTime.Now;
                    user.LastLogin = logintime;
                    await context.SaveChangesAsync();
                    ViewBag.Name = user.emailId;
                    
                }
                
                else
                {
                    ViewBag.IsUnauthorized = true;
                    return View(nameof(Index));
                }
                //String currentUser=new String("currentUser");
                HttpContext.Session.SetString(SessionName, loginViewModel.EmailID);
                //var curentUser = loginViewModel.username;
                ViewBag.Name = HttpContext.Session.GetString(SessionName);
                //ViewBag["currentUser"] = HttpContext.Session.GetString(SessionName);
                //HttpContext.Session.SetString("currentUser", loginViewModel.username);
                //var x = HttpContext.User.Identities.FirstOrDefault(ClaimTypes.Name);
            }
            // Set Session values during button click  
            //  string About()
            //{
            //    ViewBag["currentUser"] = HttpContext.Session.GetString(SessionName);
            //    return HttpContext.Session.GetString(SessionName);
            //}
            //var y = HttpContext.Session.GetString(SessionName);





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
