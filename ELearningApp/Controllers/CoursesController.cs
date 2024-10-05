using ELearningApp.Core.Assists;
using ELearningApp.Core.Models;
using ELearningApp.Models;
using ELearningApp.Scripts;
using ELearningApp.Service.DB;
using ELearningApp.Service.DB.DataHelper;
using ELearningApp.ViewModels.Courses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ELearningApp.Controllers
{
    public class CoursesController(ILogger<CoursesController> logger,
        IDataHelper<Course> coursesDataHelper,
        IDataHelper<Category> categoriesDataHelper,
        UserManager<ApplicationUser> userManager) : Controller
    {
        private readonly ILogger<CoursesController> _logger = logger;
        private readonly IDataHelper<Course> coursesDataHelper = coursesDataHelper;
        public readonly IDataHelper<Category> categoriesDataHelper = categoriesDataHelper;
        private readonly UserManager<ApplicationUser> userManager = userManager;

        public async Task<IActionResult> Index(int page = 1, int pageSize = 10, int? categoryId = null, string? search = null)
        {
            PaginatedList<Course> courses = await coursesDataHelper
                .SearchPagedWithIncludesAsync(
                    page, pageSize,
                    // (
                    // If there is filter by category
                    m => m.CategoryId == (categoryId == null ? m.CategoryId : categoryId)
                    // searching if there is
                    && (m.Title.Contains(search ?? "") || (m.Instructor != null && m.Instructor.UserName != null && m.Instructor.UserName.Contains(search ?? ""))
                    || m.Description.Contains(search ?? "")),
                    // )
                    // Includes
                    m => m.Include(m => m.Contents).Include(m => m.Category).Include(m => m.Instructor)
                );

            // Load All category to be filterd with
            var categories = await categoriesDataHelper.GetAllNoTrackingAsync();

            // Generate View Model
            var model = new CoursesViewModel
            {
                Courses = courses,
                Categories = categories.ToList(),
                CategoryId = categoryId,
                Search = search
            };

            return View(courses);
        }

        [Authorize(Roles = "Teacher, Admin")]
        public async Task<ActionResult> AddCourse()
        {
            // Get Categories
            var cateogries = await categoriesDataHelper.GetAllNoTrackingAsync();

            var model = new CourseViewModel
            {
                Categories = cateogries.ToList(),
                Course = new Course()
            };

            return View("CourseView", model);
        }

        [Authorize(Roles = "Teacher, Admin")]
        public async Task<ActionResult> EditCourse(int Id)
        {
            // Get Course
            var course = await coursesDataHelper.GetWithIncludesAsync(Id.ToString(), m => m.Include(m => m.Category));

            // Check if course exist
            if (course == null)
            {
                return RedirectToAction("Index", new { error = "Course not found" });
            }
            else
            {
                // Check if the loged in user is the instructor
                var user = await userManager.GetUserAsync(User);
                if (user?.Id != course.InstructorId)
                {
                    return RedirectToAction("Index", new { error = "You have no premission to edit this course " });
                }
            }

            // Get Categories
            var cateogries = await categoriesDataHelper.GetAllNoTrackingAsync();

            var model = new CourseViewModel
            {
                Categories = cateogries.ToList(),
                Course = course
            };

            return View("CourseView", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveData(Course course)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (course.Id == 0)
                    {
                        // New Course
                        await coursesDataHelper.AddAsync(course);
                    }
                    else
                    {
                        // Edited Course
                        // Get course from db
                        var oldCourse = await coursesDataHelper.GetWithIncludesAsync(course.Id.ToString(),
                            m => m.Include(m => m.Contents));

                        // Check if course exist
                        if (course == null)
                        {
                            return RedirectToAction("Index", new { error = "Course not found" });
                        }

                        // Check if the loged in user is the instructor
                        var user = await userManager.GetUserAsync(User);
                        if (user?.Id != oldCourse.InstructorId)
                        {
                            return RedirectToAction("Index", new { error = "You have no premission to edit this course " });
                        }

                        // Set new values
                        ObjectUpdater.UpdateValues(oldCourse, course);

                        // Update db
                        await coursesDataHelper.UpdateAsync(course);
                    }
                }
                catch
                {
                    RedirectToAction("Index", new { error = "Failed to add course, please contact with admin" });
                }
            }

            return RedirectToAction("Index", new { sucess = "Adding course done!." });
        }

        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> AproveCourse(int Id)
        {
            try
            {
                // Get Course
                var course = await coursesDataHelper.GetByIdAsync(Id.ToString());

                // Check if course exist
                if (course == null)
                {
                    return RedirectToAction("Index", new { error = "Course not found" });
                }

                course.Status = Core.enums.CourseStatus.Visible;

                // Update db
                await coursesDataHelper.UpdateAsync(course);

                return RedirectToAction("Index", new { sucess = "Course Approved" });
            }
            catch (Exception ex) 
            {
                return RedirectToAction("Index", new { error = ex.Message });
            }
        }

        [Authorize(Roles = "Teacher, Admin")]
        public async Task<ActionResult> DeleteCourse(int Id)
        {
            // get course
            var course = await coursesDataHelper.GetByIdAsync(Id.ToString());

            // Get user
            var user = await userManager.GetUserAsync(User);

            if (course != null && user != null && (User.IsInRole("Admin") || course.InstructorId == user.Id))
            {
                await coursesDataHelper.DeleteAsync(Id.ToString());
                return RedirectToAction("Index", new { sucess = "Deleted Successfuly" });
            }
            else
            {
                return RedirectToAction("Index", new { error = "Something error happend" });
            }
        }
    }
}
