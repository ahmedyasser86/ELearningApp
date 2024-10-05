using Microsoft.AspNetCore.Mvc;

namespace ELearningApp.Controllers
{
    public class UserCoursesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
