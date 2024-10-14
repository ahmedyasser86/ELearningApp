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

        public async Task<IActionResult> DeleteCourse(int? id)
        {
            var userId = (await userManager.GetUserAsync(User))?.Id;

            if (id == null)
            {
                return NotFound();
            }

            var course = await coursesDataHelper.GetByIdAsync(id.Value.ToString());

            if (course == null || course.InstructorId != userId)
            {
                return NotFound();
            }

            else
            {
                try
                {
                    await coursesDataHelper.DeleteAsync(id.Value.ToString());
                }
                catch (Exception)
                {
                    return RedirectToAction("Index", new { error = "failed to remove, contact admin" });
                }
            }

            return View(course);
        }

        public async Task<IActionResult> ToggleVisbility(int id)
        {
            var userId = (await userManager.GetUserAsync(User))?.Id;

            var course = await coursesDataHelper.GetByIdAsync(id.ToString());
            if (course == null || userId != course.InstructorId)
            {
                return RedirectToAction("Index", new { error = "Course not found" });
            }

            if (course.Status == Core.enums.CourseStatus.Visible)
                course.Status = Core.enums.CourseStatus.Hidden;
            else if(course.Status == Core.enums.CourseStatus.Hidden)
                course.Status = Core.enums.CourseStatus.Visible;
            else
            {
                return RedirectToAction("Index", new { error = "something wrong happend" });
            }

            await coursesDataHelper.UpdateAsync(course);

            return RedirectToAction("Index", new { sucess = "Toggle Visibility Done" });
        }
    }
}
