using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MedicalIntercomProject.Models;

namespace MedicalIntercomProject.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        

        public ViewResult Index()
        {
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]

        public IActionResult NewUser()
        {
            
                return View();
        }
        public void CreateNewUser()
        {
            using (var context = new UserDbContext())
            {
                var user = new User
                {

                    FirstName = "abc",
                    LastName = "xyz",
                    RoleId = 1,
                    emailId = "abc@mail.com",
                    password = "123"

                };
                context.UsersTable.Add(user);
                context.SaveChanges();
                
            }


        }

        public IActionResult Created()
        {
            return View();
        }

        
        
    }   
}


