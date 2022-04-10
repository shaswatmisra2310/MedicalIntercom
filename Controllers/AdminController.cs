using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MedicalIntercomProject.Models;
using Microsoft.AspNetCore.Authentication;
using Azure.Communication.Identity;

namespace MedicalIntercomProject.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        UserDbContext context;
        public AdminController()
        {
            context = new UserDbContext();
        }
        

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
        public async Task<IActionResult> CreateNewUser(NewUserViewModel newuserviewmodel)
        {
            //await HttpContext.Response.CompleteAsync();???
            
            {
                var user = new User();
                {
                    
                    user.FirstName = newuserviewmodel.firstname;
                    user.LastName = newuserviewmodel.lastname;
                    user.RoleId = int.Parse(newuserviewmodel.Role);                             
                    user.emailId = newuserviewmodel.username;
                    user.password = newuserviewmodel.password;
                    user.ChatIdentity = GetIdentity();

                };
                context.UsersTable.Add(user);
                await context.SaveChangesAsync();

            }
            return RedirectToAction("Index","Admin");

        }

        public string GetIdentity()
        {
            //change from hard coded string later
            string connectionString = "endpoint = https://chatcommunicationservices.communication.azure.com/;accesskey=S7oBb5x6Q4pTYgg10SZH3dXAFsrDYx1u9mxAWNjasuUkHAgtv0fse/zwkoJVmGHU7XjC0o6CcAs4ip8OpxYkFg==";
            var client = new CommunicationIdentityClient("endpoint = https://chatcommunicationservices.communication.azure.com/;accesskey=S7oBb5x6Q4pTYgg10SZH3dXAFsrDYx1u9mxAWNjasuUkHAgtv0fse/zwkoJVmGHU7XjC0o6CcAs4ip8OpxYkFg==");
            var identityResponse = client.CreateUser();
            var identity = identityResponse.Value;
            ViewBag.identity = identity.Id;
            return identity.Id;
        }

        public IActionResult OpenChat()
        {
            ViewBag.Identity = ViewBag.identity;
            return RedirectToAction("Index", "Chat");
        }

    }   
}


