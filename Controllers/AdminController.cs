using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MedicalIntercomProject.Models;
using Microsoft.AspNetCore.Authentication;

namespace MedicalIntercomProject.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        

        public ViewResult Index()
        {
            return View();
        }
        //[HttpPost]
        //[Authorize(Roles = "Admin")]

        public IActionResult NewUser()
        {
            
                return View();
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(NewUserViewModel newuserviewmodel)
        {
            await HttpContext.SignOutAsync();
            using (var context = new UserDbContext())
            {
                var user = new User();
                {

                    user.FirstName = newuserviewmodel.firstname;
                    user.LastName = newuserviewmodel.lastname;
                    user.RoleId = 2;                             //to be changed
                    user.emailId = newuserviewmodel.username;
                    user.password = newuserviewmodel.password;
                    user.ChatIdentity = newuserviewmodel.ChatId;

                };
                context.UsersTable.Add(user);
                context.SaveChanges();
                
            }
            return RedirectToAction("Index","Admin");

        }

        
        
    }   
}


