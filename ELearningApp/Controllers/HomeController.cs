using Microsoft.AspNetCore.Mvc;

namespace ELearningApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> GetCategories()
        {
            return Json("");
        }
    }
}
