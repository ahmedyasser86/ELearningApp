using ELearningApp.Core.Models;
using ELearningApp.Service.DB.DataHelper;
using Microsoft.AspNetCore.Mvc;

namespace ELearningApp.Controllers
{
    public class HomeController(IDataHelper<Course> coursesDataHelper, IDataHelper<Category> categoryDataHelper) : Controller
    {
        private readonly IDataHelper<Course> coursesDataHelper = coursesDataHelper;
        private readonly IDataHelper<Category> categoryDataHelper = categoryDataHelper;

        public IActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> GetCategories()
        {
            // Get Categories
            var categories = (await categoryDataHelper.GetAllNoTrackingAsync()).Select(m => new
            {
                m.Id,
                m.Name
            });

            return Json(categories);
        }

        public async Task<ActionResult> GetCourses()
        {
            // Get Courses
            var courses = await coursesDataHelper.SearchPagedWithIncludesInOrderAsync(1, 6,
                m => m.Status == Core.enums.CourseStatus.Visible,
                m => m.Include(m => m.Category).Include(m => m.Instructor),
                true, m => m.Id
            );

            var result = courses.Items.Select(course => new
            {
                course.Id,
                course.Title,
                course.Description,
                ImagePath = course.ImagePath ?? "img/default-course.jpg", // مسار الصورة الافتراضي
                Instructor = course.Instructor != null ? new { Name = course.Instructor.FullName } : null,
                Rating = 4.5, // يمكنك التعديل لتأخذ التقييم الفعلي من بياناتك
                ReviewsCount = 250 // نفس الشيء هنا بالنسبة لعدد المراجعات
            });

            return Json(result); ;
        }
        public IActionResult Privacy()
        {
            return View();
        }
    }
}
