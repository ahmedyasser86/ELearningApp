using ELearningApp.Core.Models;
using ELearningApp.Service.DB.DataHelper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace ELearningApp.Controllers
{
    public class UserCoursesController : Controller
    {
        private readonly ILogger<UserCoursesController> _logger;
        private readonly IDataHelper<UserCourse> userCoursesDataHelper;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserCoursesController(ILogger<UserCoursesController> logger, IDataHelper<UserCourse> userCoursesDataHelper)
        {
            _logger = logger;
            this.userCoursesDataHelper = userCoursesDataHelper;
        }

        // عرض الدورات المسجلة للمستخدم
        public async Task<IActionResult> Index(int page = 1, int pageSize = 10)
        {
            var user = await _userManager.GetUserAsync(User);

            // Here Check if null..
            string userId = user?.Id;

            // Using search methode to get current user's courses only
            var userCourses = await userCoursesDataHelper.SearchPagedWithIncludesAsync(1, 10,
                m => m.UserId == userId,
                m => m.Include(m => m.Course).ThenInclude(m => ((Course)m).Instructor));


            return View(userCourses);
        }

    //    // تسجيل المستخدم في دورة
    //    [HttpPost]
    //    [ValidateAntiForgeryToken]
    //    public async Task<IActionResult> Register(int courseId)
    //    {
    //        var user = await _userManager.GetUserAsync(User);

    //        // Here Check if null..
    //        string userId = user?.Id;

    //        //// تحقق إذا كان المستخدم مسجلاً بالفعل في هذه الدورة
    //        //var userCourses = await userCoursesDataHelper.GetAsync(uc => uc.CourseId == courseId && uc.userId == userId);
    //        //if (userCourse != null)
    //        //{
    //        //    ModelState.AddModelError("", "You are already registered in this course.");
    //        //    return RedirectToAction(nameof(Index));
    //        //}

    //        // إضافة التسجيل في الدورة
    //        var userCourse = new UserCourse
    //        {
    //            CourseId = courseId,
    //            UserId = userId,
    //            EnrollDate = DateTime.Now
    //        };

    //        await userCoursesDataHelper.AddAsync(userCourse);

    //        return RedirectToAction(nameof(Index));
    //    }

    //    // إلغاء تسجيل المستخدم من دورة
    //    [HttpPost]
    //    [ValidateAntiForgeryToken]
    //    public async Task<IActionResult> Unregister(int courseId)
    //    {
    //        //var user = await _userManager.GetUserAsync(User);

    //        //// Here Check if null..
    //        //string userId = user?.Id;

    //        //// البحث عن التسجيل المراد إلغاؤه
    //        //var userCourse = await userCoursesDataHelper.GetAsync(uc => uc.CourseId == courseId && GetUserId(uc) == userId);

    //        //if (userCourse != null)
    //        //{
    //        //    await userCoursesDataHelper.DeleteAsync(userCourse);
    //        //}

    //        return RedirectToAction(nameof(Index));
    //    }

    }
}
