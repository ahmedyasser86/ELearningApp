using ELearningApp.Core.Models;
using ELearningApp.Service.DB.DataHelper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ELearningApp.Controllers
{
    [Authorize(Roles = "Admin, Teacher")]
    public class UserCoursesController : Controller
    {
        private readonly IDataHelper<Course> _coursesDataHelper;
        private readonly IDataHelper<Content> _contentDataHelper;

        public UserCoursesController(IDataHelper<Course> coursesDataHelper, IDataHelper<Content> contentDataHelper)
        {
            _coursesDataHelper = coursesDataHelper;
            _contentDataHelper = contentDataHelper;
        }

        // Index: عرض الدورات الخاصة بالمستخدم
        public async Task<ActionResult> Index(int page = 1, int pageSize = 10)
        {
            var courses = await _coursesDataHelper.GetPagedAsync(page, pageSize);
            return View(courses);
        }

        // عرض تفاصيل دورة معينة
        public async Task<ActionResult> Details(int id)
        {
            var course = await _coursesDataHelper.GetByIdAsync(id.ToString());

            if (course == null)
            {
                return RedirectToAction("Index", new { error = "Course not found" });
            }

            return View(course);
        }

        // إضافة دورة جديدة
        public ActionResult Add()
        {

            return View("CourseForm", new Course());
        }

        // تعديل دورة موجودة
        public async Task<ActionResult> Edit(int id)
        {
            var course = await _coursesDataHelper.GetByIdAsync(id.ToString());

            if (course == null)
            {
                return RedirectToAction("Index", new { error = "Course not found" });
            }

            return View("CourseForm", course);
        }

        // حفظ البيانات (إضافة أو تعديل)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Save(Course course)
        {
            if (ModelState.IsValid)
            {
                if (course.Id == 0)
                {
                    // إضافة دورة جديدة
                    await _coursesDataHelper.AddAsync(course);
                }
                else
                {
                    // تعديل دورة موجودة
                    await _coursesDataHelper.UpdateAsync(course);
                }

                return RedirectToAction("Index", new { success = "Course saved successfully!" });
            }

            return View("CourseForm", course);
        }

        // حذف دورة
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var course = await _coursesDataHelper.GetByIdAsync(id.ToString());

                if (course == null)
                {
                    return RedirectToAction("Index", new { error = "Course not found" });
                }

                await _coursesDataHelper.DeleteAsync(id.ToString());

                return RedirectToAction("Index", new { success = "Course deleted successfully!" });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", new { error = ex.Message });
            }
        }

        // إضافة محتوى لدورة معينة
        public async Task<ActionResult> AddContent(int courseId)
        {
            var course = await _coursesDataHelper.GetByIdAsync(courseId.ToString());

            if (course == null)
            {
                return RedirectToAction("Index", new { error = "Course not found" });
            }

            var content = new Content { CourseId = courseId };
            return View("ContentForm", content);
        }

        // تعديل محتوى موجود لدورة
        public async Task<ActionResult> EditContent(int contentId)
        {
            var content = await _contentDataHelper.GetByIdAsync(contentId.ToString());

            if (content == null)
            {
                return RedirectToAction("Index", new { error = "Content not found" });
            }

            return View("ContentForm", content);
        }

        // حفظ محتوى (إضافة أو تعديل)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveContent(Content content)
        {
            if (ModelState.IsValid)
            {
                if (content.Id == 0)
                {
                    await _contentDataHelper.AddAsync(content);
                }
                else
                {
                    await _contentDataHelper.UpdateAsync(content);
                }

                return RedirectToAction("Details", new { id = content.CourseId, success = "Content saved successfully!" });
            }

            return View("ContentForm", content);
        }
    }
}
