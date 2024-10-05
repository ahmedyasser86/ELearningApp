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

        public async Task<ActionResult> Index(int page = 1, int pageSize = 10, string? search)
        {
            var categories = await categoriesDataHelper.GetPagedAsync(page, pageSize);

            return View(categories);
        }
    }
}
