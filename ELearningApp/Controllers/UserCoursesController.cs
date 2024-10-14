using ELearningApp.Core.Models;
using ELearningApp.Service.DB.DataHelper;
using ELearningApp.ViewModels.Contents;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace ELearningApp.Controllers
{
    [Authorize]
    public class UserCoursesController(IDataHelper<UserCourse> coursesDataHelper, 
        IDataHelper<UserProgress> userProgressDataHelper,
        UserManager<ApplicationUser> userManager,
        IDataHelper<Content> contentDataHlper, 
        IDataHelper<Course> courseDataHelper) : Controller
    {
        private readonly IDataHelper<UserCourse> userCoursesDataHelper = coursesDataHelper;
        private readonly IDataHelper<UserProgress> userProgressDataHelper = userProgressDataHelper;
        private readonly IDataHelper<Content> contentDataHlper = contentDataHlper;
        private readonly IDataHelper<Course> courseDataHelper = courseDataHelper;
        private readonly UserManager<ApplicationUser> userManager = userManager;

        // Index: عرض الدورات الخاصة بالمستخدم
        public async Task<ActionResult> Index(int page = 1, int pageSize = 10)
        {
            // Get Loged in User Id
            var userId = userManager.GetUserId(User);

            var userCourses = await userCoursesDataHelper.SearchPagedWithIncludesAsync(
                page, pageSize,
                m => m.ApplicationUserId == userId, // Search by User Id
                m => m.Include(m => m.Course) // Include Course data to get name, instructor name, category etc
                .ThenInclude(m => ((Course)m).Instructor) // to get instructor data like Name
                .Include(m => m.Course).ThenInclude(m => ((Course)m).Contents) // to get course contents
                .Include(m => m.Progresses) // To get user progress
                );

            return View(userCourses);
        }

        public async Task<ActionResult> Watch(int courseId, int? contentId = null)
        {
            // Get user Id
            var userId = userManager.GetUserId(User);

            // Get userCourse
            var userCourse = (await userCoursesDataHelper.SearchPagedWithIncludesAsync(
                1, 1,
                m => m.ApplicationUserId == userId && m.CourseId == courseId,
                m => m.Include(m => m.Course).ThenInclude(m => ((Course)m).Contents)
                )).Items.FirstOrDefault();

            // UserProgress
            var userProgress = (await userProgressDataHelper.SearchPagedAsync(
                1, int.MaxValue,
                m => m.UserCourseId == userCourse.Id
                )).Items;

            if(userCourse == null)
            {
                return RedirectToAction("Index", new { error = "Somthing error Happend" });
            }

            Content? content;
            if(contentId == null)
            {
                content = userCourse.Course?.Contents?
                    .Where(m => !userProgress.Any(p => p.ContentId == m.Id))
                    .OrderBy(m => m.OrderNumber).FirstOrDefault();
            }
            else
            {
                content = userCourse.Course?.Contents?.FirstOrDefault(m => m.Id == contentId);
            }
            
            // Check if content is null
            if (content == null)
            {
                // That's mean that the user watched all content or there is no content yet
                // Load the first content
                content = userCourse.Course?.Contents?.OrderBy(m => m.OrderNumber).FirstOrDefault();

                if (content == null)
                    return RedirectToAction("Index", new { error = "Somthing error Happend" });
            }

            // Mark watched content as watched
            foreach (var item in userCourse.Course?.Contents ?? [])
            {
                if(userProgress.Any(m => m.ContentId == item.Id))
                {
                    item.IsWatched = true;
                }
            }

            var viewModel = new ContentViewModel
            {
                Content = content,
                Contents = userCourse.Course?.Contents?.OrderBy(m => m.OrderNumber).ToList(),
                NextContentId = userCourse.Course?.Contents?.Where(m => m.OrderNumber > content.OrderNumber)
                    .OrderBy(m => m.OrderNumber).FirstOrDefault()?.Id,
                PreviousContentId = userCourse.Course?.Contents?.Where(m => m.OrderNumber < content.OrderNumber)
                    .OrderByDescending(m => m.OrderNumber).FirstOrDefault()?.Id,
                UserCourseId = userCourse.Id
            };

            return View(viewModel);
        }

        public async Task<ActionResult> MarkAsWatched(int contentId, int userCourseId)
        {
            try
            {
                // Get Content Course Id
                var content = await contentDataHlper.GetWithIncludesAsync(contentId.ToString(), m => m.Include(m => m.Course).ThenInclude(m => ((Course)m).Students));

                // Get User Id
                var userId = userManager.GetUserId(User);

                // Check if user is null or not in student list
                if (userId == null || userCourseId == 0 || content?.Course?.Students == null || !content.Course.Students.Any(m => m.ApplicationUserId == userId))
                {
                    return Json("Failed");
                }

                // Add New Progress
                await userProgressDataHelper.AddAsync(new UserProgress
                {
                    UserCourseId = userCourseId,
                    ContentId = contentId,
                });

                return Json("Done");
            }
            catch (Exception)
            {
                return Json("Failed");
            }
        }

        // إضافة دورة جديدة
        public async Task<ActionResult> Enroll(int courseId)
        {
            try
            {
                // get user Id
                var userId = userManager.GetUserId(User);

                // check if course exist
                var course = await courseDataHelper.GetByIdAsync(courseId.ToString());

                if (userId == null || course == null)
                {
                    throw new Exception();
                }

                // check if user already enrolled
                var userCourse = await userCoursesDataHelper.SearchAsync(m => m.ApplicationUserId == userId && m.CourseId == courseId);
                if (userCourse.Any())
                {
                    return RedirectToAction($"Watch", new { courseId });
                }

                // Enroll
                await userCoursesDataHelper.AddAsync(new UserCourse
                {
                    ApplicationUserId = userId,
                    CourseId = courseId,
                    EnrollDate = DateTime.Now,
                });

                return RedirectToAction($"Watch", new { courseId });
            }
            catch (Exception)
            {
                return RedirectToAction("Index", new { error = "Something error happend" });
            }
        }

        // حذف دورة
        public async Task<ActionResult> UnEnroll(int userCourseId)
        {
            try
            {
                var course = await userCoursesDataHelper.GetByIdAsync(userCourseId.ToString());

                if (course == null)
                {
                    return RedirectToAction("Index", new { error = "Course not found" });
                }

                await userCoursesDataHelper.DeleteAsync(userCourseId.ToString());

                return RedirectToAction("Index", new { success = "Course deleted successfully!" });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", new { error = ex.Message });
            }
        }

        public IActionResult Index2()
        {
            return View();
        }
    }
}
