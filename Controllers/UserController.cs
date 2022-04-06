using MedicalIntercomProject.Models;
using Microsoft.AspNetCore.Mvc;

namespace MedicalIntercomProject.Controllers
{
    public class UserController : Controller
    {
        UserDbContext db;
        //context = new UserDbContext();
        public UserController(UserDbContext _db)
        {
            db = _db;
        }
        public IActionResult Index()
        {
            IEnumerable<User> users = db.UsersTable.Select(s => s).ToList();
            return View(users);
        }

        public IActionResult Delete(int Id)
        {
            User user = db.UsersTable.FirstOrDefault(u => u.Id == Id);
            if (user != null)
            {
                db.Remove(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult NewUser()
        {
            return RedirectToAction("NewUser", "Admin");
        }
    }
}
