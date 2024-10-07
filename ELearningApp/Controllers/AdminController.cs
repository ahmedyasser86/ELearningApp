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

    public class AdminController(ILogger<AdminController> logger,
        IDataHelper<ApplicationUser> usersDataHelper,
        RoleManager<IdentityRole> roleManager,
        UserManager<ApplicationUser> userManager) : Controller
    {
        private readonly ILogger<AdminController> _logger = logger;
        private readonly IDataHelper<ApplicationUser> usersDataHelper = usersDataHelper;
        private readonly RoleManager<IdentityRole> roleManager = roleManager;
        private readonly UserManager<ApplicationUser> userManager = userManager;

        // عرض جميع المستخدمين
        public async Task<IActionResult> Index(string search = "")
        {
            var users = await usersDataHelper.GetAllNoTrackingAsync();

            if (!string.IsNullOrEmpty(search))
            {
                users = users.Where(predicate: u => u.Id.Contains(search) || u.UserName.Contains(search) || u.Email.Contains(search)).ToList();
            }

            return View(users);
        }
        // تعديل بيانات المستخدم
        public async Task<IActionResult> Edit(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            if (user == null)
            {
                return RedirectToAction("Index", new { error = "User not found" });
            }

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ApplicationUser model)
        {
            var user = await userManager.FindByIdAsync(model.Id);
            if (user == null)
            {
                return RedirectToAction("Index", new { error = "User not found" });
            }

            user.UserName = model.UserName;
            user.Email = model.Email;
            user.PhoneNumber = model.PhoneNumber;

            var result = await userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return RedirectToAction("Index", new { success = "User updated successfully" });
            }

            return View(user);
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

    //    // إدارة الأدوار للمستخدمين
    //    public async Task<IActionResult> ManageRoles(string id)
    //    {
    //        var user = await userManager.FindByIdAsync(id);
    //        if (user == null)
    //        {
    //            return RedirectToAction("Index", new { error = "User not found" });
    //        }

    //        var roles = await roleManager.Roles.ToListAsync();
    //        var userRoles = await userManager.GetRolesAsync(user);

    //        var model = new roleManager
    //        {
    //            UserId = user.Id,
    //            AvailableRoles = roles,
    //            UserRoles = userRoles.ToList()
    //        };

    //        return View(model);
    //    }

    //    [HttpPost]
    //    // عرض الأدوار وإدارتها للمستخدمين
    //    [HttpPost]
    //    public async Task<IActionResult> ManageRoles(roleManager model)
    //    {
    //        var user = await userManager.FindByIdAsync(model.UserId);
    //        if (user == null)
    //        {
    //            return RedirectToAction("Index", new { error = "User not found" });
    //        }

    //        // الحصول على الأدوار الحالية للمستخدم
    //        var userRoles = await userManager.GetRolesAsync(user);

    //        // تحديد الأدوار التي يجب إضافتها
    //        var rolesToAdd = model.SelectedRoles.Except(userRoles).ToList();

    //        // تحديد الأدوار التي يجب إزالتها
    //        var rolesToRemove = userRoles.Except(model.SelectedRoles).ToList();

    //        // إضافة الأدوار
    //        if (rolesToAdd.Any())
    //        {
    //            await userManager.AddToRolesAsync(user, rolesToAdd);
    //        }

    //        // إزالة الأدوار
    //        if (rolesToRemove.Any())
    //        {
    //            await userManager.RemoveFromRolesAsync(user, rolesToRemove);
    //        }

    //        return RedirectToAction("Index", new { success = "Roles updated successfully" });
    //    }

    }
}
