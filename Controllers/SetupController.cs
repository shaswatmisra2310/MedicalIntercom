using MedicalIntercomProject.Models;
using Microsoft.AspNetCore.Mvc;

namespace MedicalIntercomProject.Controllers
{
    public class SetupController : Controller
    {
        public IActionResult Index()
        {
            LayoutModel layoutmodel = new LayoutModel();
            layoutmodel.currentUser = (string)TempData["currentusername"];

            return RedirectToAction("Loggedinuser", "Home");
        }
    }
}
