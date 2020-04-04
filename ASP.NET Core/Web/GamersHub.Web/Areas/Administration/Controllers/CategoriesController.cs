﻿using System.Linq;
using System.Threading.Tasks;
using GamersHub.Common;
using GamersHub.Services.Data;
using GamersHub.Services.Data.Categories;
using GamersHub.Services.Data.Forums;
using GamersHub.Web.ViewModels.Administration.Categories;
using Microsoft.AspNetCore.Mvc;

namespace GamersHub.Web.Areas.Administration.Controllers
{
    public class CategoriesController : AdministrationController
    {
        private readonly ICategoriesService categoriesService;
        private readonly IForumsService forumsService;

        public CategoriesController(ICategoriesService categoriesService, IForumsService forumsService)
        {
            this.categoriesService = categoriesService;
            this.forumsService = forumsService;
        }

        public IActionResult Index()
        {
            var categories = this.categoriesService
                .GetAll<CategoryViewModel>();

            var viewModel = new CategoryIndexViewModel {Categories = categories};

            return this.View(viewModel);
        }

        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryCreateInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(inputModel);
            }

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

        public IActionResult Edit(int id)
        {
            var category = this.categoriesService.GetById<CategoryEditInputModel>(id);

            if (category == null)
            {
                return this.NotFound();
            }

            var forums = this.forumsService.GetAllByCategoryId<ForumEditViewModel>(id).ToArray();

            var viewModel = new CategoryAdministrationEditInputModel
            {
                Category = category,
                Forums = forums,
            };

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CategoryAdministrationEditInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                var category = this.categoriesService.GetById<CategoryEditInputModel>(input.Id);
                var forums = this.forumsService.GetAllByCategoryId<ForumEditViewModel>(input.Id).ToArray();

                input.Category = category;
                input.Forums = forums;

                return this.View(input);
            }

            var categoryId = await this.categoriesService.EditAsync(input.Id, input.Category.Name, input.Category.Description, input.ForumIds, input.AreSelected);

            if (categoryId == -1)
            {
                return this.NotFound();
            }

            if (categoryId == 0)
            {
                this.ModelState.AddModelError(
                    "Category.Name",
                    string.Format(GlobalConstants.CategoryNameAlreadyExistsErrorMessage, input.Category.Name));
            }

            if (!this.ModelState.IsValid)
            {
                var category = this.categoriesService.GetById<CategoryEditInputModel>(input.Id);
                var forums = this.forumsService.GetAllByCategoryId<ForumEditViewModel>(input.Id).ToArray();

                input.Category = category;
                input.Forums = forums;

                return this.View(input);
            }

            return this.RedirectToAction(nameof(this.Index));
        }


        public IActionResult Delete(int id)
        {
            var viewModel = this.categoriesService.GetById<CategoryDeleteViewModel>(id);

            if (viewModel == null)
            {
                return this.NotFound();
            }

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(CategoryDeleteViewModel input)
        {
            await this.categoriesService.DeleteAsync(input.Id);

            return this.RedirectToAction(nameof(this.Index));
        }
    }
}