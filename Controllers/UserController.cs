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

        public ViewResult Details(int id)
        {
            User user = db.UsersTable.SingleOrDefault(s => s.Id == id);
            return View(user);
        }

        public ActionResult Edit(int Id)
        {
            User user =db.UsersTable.Where(s => s.Id == Id).SingleOrDefault();
            return View(user);
        }

        [HttpPost]
        public ActionResult Edit(User user)
        {
            User userx = db.UsersTable.Where(db => db.Id == user.Id).SingleOrDefault();
            if(user != null)
            {
                db.Entry(user).CurrentValues.SetValues(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
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
