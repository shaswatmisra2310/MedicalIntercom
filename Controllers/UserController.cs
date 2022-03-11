//using MedicalIntercomProject.Models;
//using Microsoft.AspNetCore.Mvc;

//namespace MedicalIntercomProject.Controllers
//{
//    public class UserController : Controller
//    {
//        UserDbContext context;
//        //context = new UserDbContext();
//        public UserController(UserDbContext _context)
//        {
//            context =_context;
//        }
//        public IActionResult Index()
//        {
//            IEnumerable<User> users = context.UsersTable.Select(s => s).ToList();
//            return View(users);
//        }

//        //public IActionResult Delete(int Id)
//        //{
//        //    User user = context.UsersTable.FirstOrDefault(u => u.Id == Id);
//        //    if(user != null)
//        //    {
//        //        context.Remove(user);
//        //        context.SaveChanges();
//        //        return RedirectToAction("Index");
//        //    }
//        //    return View();
//        //}
//    }
//}
