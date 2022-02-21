using Microsoft.AspNetCore.Mvc;

namespace MedicalIntercomProject.Controllers
{
    public class HomeController1 : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
