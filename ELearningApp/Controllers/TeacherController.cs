using ELearningApp.Core.Models;
using ELearningApp.Service.DB.DataHelper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ELearningApp.Controllers
{
    [Authorize]
    public class TeacherController(IDataHelper<Course> coursesDataHelper, UserManager<ApplicationUser> userManager) : Controller
    {
        private readonly IDataHelper<Course> coursesDataHelper = coursesDataHelper;
        private readonly UserManager<ApplicationUser> userManager = userManager;

        public async Task<IActionResult> Index(int page = 1, int pageSize = 10)
        {
            // Get User
            var user = await userManager.GetUserAsync(User);

            // Get Courses
            var courses = await coursesDataHelper.SearchPagedAsync(page, pageSize,
                m => m.InstructorId == user.Id
                );

            return View(courses);
        }
    }
}
