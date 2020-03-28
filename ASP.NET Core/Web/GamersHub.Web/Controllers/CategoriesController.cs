using System;
using System.Linq;
using System.Threading.Tasks;
using GamersHub.Common;
using GamersHub.Services.Data;
using GamersHub.Web.ViewModels.Categories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GamersHub.Web.Controllers
{
    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    public class CategoriesController : BaseController
    {
        private readonly ICategoriesService categoriesService;

        public CategoriesController(ICategoriesService categoriesService)
        {
            this.categoriesService = categoriesService;
        }

        public IActionResult Index()
        {
            var categories = this.categoriesService
                .GetAll<CategoryViewModel>()
                .OrderByDescending(x => x.PostsCount)
                .ThenByDescending(x => x.CategoryForumsCount);

            var viewModel = new CategoryIndexViewModel {Categories = categories};

            return this.View(viewModel);
        }

        public IActionResult ByName(string name, int id)
        {
            var viewModel = this.categoriesService.GetByNameAndForumId(name, id);

            if (viewModel == null)
            {
                return this.NotFound();
            }

            return this.View(viewModel);
        }


        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryCreateInputModel inputModel)
        {
            var categoryId = await this.categoriesService.CreateAsync(inputModel.Name, inputModel.Description);

            if (categoryId == 0)
            {
                this.ModelState.AddModelError(
                    nameof(inputModel.Name),
                    string.Format(GlobalConstants.CategoryNameAlreadyExistsErrorMessage, inputModel.Name));
            }

            if (!this.ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            return this.RedirectToAction(nameof(this.Index));
        }
    }
}