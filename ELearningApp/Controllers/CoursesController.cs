using ELearningApp.Core.Assists;
using ELearningApp.Core.Models;
using ELearningApp.Models;
using ELearningApp.Service.DB;
using ELearningApp.Service.DB.DataHelper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ELearningApp.Controllers
{
    public class CoursesController : Controller
    {
        private readonly ILogger<CoursesController> _logger;
        private readonly IDataHelper<Course> coursesDataHelper;

        public CoursesController(ILogger<CoursesController> logger, IDataHelper<Course> coursesDataHelper)
        {
            _logger = logger;
            this.coursesDataHelper = coursesDataHelper;
        }

        public async Task<IActionResult> Index(int page = 1, int pageSize = 10)
        {
            PaginatedList<Course> courses = await coursesDataHelper
                .GetPagedWithIncludesAsync(
                page, pageSize,
                m => m.Include(m => m.Contents).Include(m => m.Category).Include(m => m.Instructor)
                );

            return View(courses);
        }

        [Authorize(Roles = "Teacher, Admin")]
        public Task<ActionResult> AddCourse()
        {
            throw new NotImplementedException();
        }

        [Authorize(Roles = "Teacher, Admin")]
        public Task<ActionResult> EditCourse(int Id)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public Task<ActionResult> SaveData(Course course) 
        {
            if(course.Id == 0)
            {
                // New Course
                // TODO: Add to db
            }
            else
            {
                // Edited Course
                // TODO: Update db
            }

            throw new NotImplementedException();
        }

        [Authorize(Roles = "Admin")]
        public Task<ActionResult> AproveCourse(Course course)
        {
            throw new NotImplementedException();
        }

        [Authorize(Roles = "Teacher, Admin")]
        public Task<ActionResult> DeleteCourse(int Id)
        {
            throw new NotImplementedException();
        }


        public IActionResult Privacy()
        {
            return View();
        }
    }
}
