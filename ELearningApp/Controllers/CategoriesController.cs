using ELearningApp.Core.Models;
using ELearningApp.Service.DB.DataHelper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ELearningApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CategoriesController(IDataHelper<Category> categoriesDataHelper) : Controller
    {
        private readonly IDataHelper<Category> categoriesDataHelper = categoriesDataHelper;

        // Index
        public async Task<ActionResult> Index(int page = 1, int pageSize = 10, string? search = null)
        {
            var categories = await categoriesDataHelper.GetPagedAsync(page, pageSize);

            return View(categories);
        }

        // Add Category
        public ActionResult Add()
        {
            return View("CategoryView");
        }

        // Edit Category
        public async Task<ActionResult> Edit(int Id)
        {
            var category = await categoriesDataHelper.GetByIdAsync(Id.ToString());

            if (category == null)
            {
                return RedirectToAction("Index", new { error = "Category not found" });
            }

            return View("CategoryView", category);
        }

        // Save Data
        public async Task<ActionResult> Save(Category category)
        {
            if (ModelState.IsValid)
            {
                if (category.Id == 0)
                {
                    await categoriesDataHelper.AddAsync(category);
                }
                else
                {
                    await categoriesDataHelper.UpdateAsync(category);
                }

                return RedirectToAction("Index", new { success = "Category Saved.!" });
            }

            return View("CategoryView", category);
        }

        // Delete Category
        public async Task<ActionResult> Delete(int Id)
        {
            try
            {
                var category = await categoriesDataHelper.GetByIdAsync(Id.ToString());

                if (category == null)
                {
                    return RedirectToAction("Index", new { error = "Category not found" });
                }

                await categoriesDataHelper.DeleteAsync(Id.ToString());

                return RedirectToAction("Index", new { success = "Category Deleted.!" });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", new { error = ex.Message });
            }
        }
    }
}
