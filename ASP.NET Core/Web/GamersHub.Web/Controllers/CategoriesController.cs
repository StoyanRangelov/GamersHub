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

            var viewModel = new CategoryIndexViewModel { Categories = categories};

            return this.View(viewModel);
        }

        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryCreateInputModel inputModel)
        {
            bool alreadyExists = this.categoriesService.CheckIfExistsByName(inputModel.Name);

            if (alreadyExists)
            {
                this.ModelState.AddModelError(
                    nameof(inputModel.Name),
                    string.Format(GlobalConstants.CategoryNameAlreadyExistsErrorMessage, inputModel.Name));
            }

            if (!this.ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            await this.categoriesService.CreateAsync(inputModel.Name, inputModel.Description);

            return this.RedirectToAction(nameof(this.Index));
        }
    }
}
