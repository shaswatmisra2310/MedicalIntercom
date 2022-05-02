using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MedicalIntercomProject.Models;
using Microsoft.AspNetCore.Authentication;
using Azure.Communication.Identity;
using System.Text;
using System.Security.Cryptography;
using System.Security.Claims;

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
        [Authorize(Roles = "Admin")]

        public IActionResult NewUser()
        {
            
                return View();
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateNewUser(NewUserViewModel newuserviewmodel)
        {
            //newuserviewmodel.currentUserEmailId = 
            SHA256 SHA256instance = SHA256.Create();
            //await HttpContext.Response.CompleteAsync();???
            //byte[] bytes = Encoding.ASCII.GetBytes(newuserviewmodel.password);
            //var passwordbytes = SHA256instance.ComputeHash(bytes);
            //var stringpassword= String.Join("", bytes);
            var temp = HttpContext.User.Claims.First(x => x.Type == ClaimTypes.Email);
            var temp2 = temp;
            User userx = context.UsersTable.Where(s => s.emailId == temp2.Value).SingleOrDefault();

            {
                var user = new User();
                {
                    
                    user.FirstName = newuserviewmodel.firstname;
                    user.LastName = newuserviewmodel.lastname;
                    user.RoleId = int.Parse(newuserviewmodel.Role);                             
                    user.emailId = newuserviewmodel.username;
                    user.password = newuserviewmodel.password;
                    user.ChatIdentity = GetIdentity();
                    user.CreatedBy = userx.emailId;
                    var createtime = DateTime.Now;
                    user.CreatedAt = createtime;
                    
                };
                context.UsersTable.Add(user);
                await context.SaveChangesAsync();

            }
            return RedirectToAction("Index","Admin");

        }

        public string GetIdentity()
        {
            //change from hard coded string later
            //string connectionString = "endpoint=https://commservicepoc.communication.azure.com/;accesskey=aW9DN0hy3PDGbum/fdGO05uL7SAJxQkFGRYDeLtxZiAmG9+LjVma/9fc0xD9bArpppZBRgj7EpV/OKzK5EvGIQ==";
            var client = new CommunicationIdentityClient("endpoint=https://commservicepoc.communication.azure.com/;accesskey=aW9DN0hy3PDGbum/fdGO05uL7SAJxQkFGRYDeLtxZiAmG9+LjVma/9fc0xD9bArpppZBRgj7EpV/OKzK5EvGIQ==");
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


