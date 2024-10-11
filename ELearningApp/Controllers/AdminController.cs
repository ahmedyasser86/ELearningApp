using ELearningApp.Core.Assists;
using ELearningApp.Core.Models;
using ELearningApp.Models;
using ELearningApp.Scripts;
using ELearningApp.Service.DB;
using ELearningApp.Service.DB.DataHelper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace ELearningApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController(ILogger<AdminController> logger,
        IDataHelper<ApplicationUser> usersDataHelper,
        UserManager<ApplicationUser> userManager) : Controller
    {
        private readonly ILogger<AdminController> _logger = logger;
        private readonly IDataHelper<ApplicationUser> usersDataHelper = usersDataHelper;
        private readonly UserManager<ApplicationUser> userManager = userManager;

        // عرض جميع المستخدمين
        public async Task<IActionResult> Index(int page = 1, int pagesize = 10, string search = "")
        {
            return RedirectToAction($"UserManagement", new { page, pagesize, search });
        }
        
        public async Task<IActionResult> UserManagement(int page = 1, int pagesize = 10, string search = "")
        {
            PaginatedList<ApplicationUser> users;
            if (string.IsNullOrEmpty(search))
            {
                users = await usersDataHelper.GetPagedAsync(page, pagesize);
            }
            else
            {
                users = await usersDataHelper.SearchPagedAsync(1, 10, m => m.UserName == search);
            }

            return View(users);
        }

        // حذف مستخدم
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            if (user == null)
            {
                return RedirectToAction("Index", new { error = "User not found" });
            }

            var result = await userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", new { success = "User deleted successfully" });
            }

            return RedirectToAction("Index", new { error = "Error deleting user" });
        }

        // Make Admin
        public async Task<IActionResult> MakeAdmin(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            if (user == null)
            {
                return RedirectToAction("Index", new { error = "User not found" });
            }

            var result = await userManager.AddToRoleAsync(user, "Admin");
            if (result.Succeeded)
            {
                return RedirectToAction("Index", new { success = "User made admin successfully" });
            }

            return RedirectToAction("Index", new { error = "Error making user admin" });
        }

    }
}
