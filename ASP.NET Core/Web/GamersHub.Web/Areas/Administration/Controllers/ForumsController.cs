﻿using System.Linq;
using System.Threading.Tasks;
using GamersHub.Common;
using GamersHub.Services.Data;
using GamersHub.Services.Data.Categories;
using GamersHub.Services.Data.Forums;
using GamersHub.Web.ViewModels.Administration.Forums;
using Microsoft.AspNetCore.Mvc;

namespace GamersHub.Web.Areas.Administration.Controllers
{
    public class ForumsController : AdministrationController
    {
        private readonly IForumsService forumsService;
        private readonly ICategoriesService categoriesService;

        public ForumsController(IForumsService forumsService, ICategoriesService categoriesService)
        {
            this.forumsService = forumsService;
            this.categoriesService = categoriesService;
        }

        public IActionResult Index()
        {
            var forums = this.forumsService.GetAll<ForumAdministrationViewModel>();

            var viewModel = new ForumAdministrationIndexViewModel {Forums = forums};

            return this.View(viewModel);
        }


        public IActionResult Create()
        {
            return this.View();
        }


        [HttpPost]
        public async Task<IActionResult> Create(ForumCreateInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            var forumId = await this.forumsService.CreateAsync(inputModel.Name);

            if (forumId == 0)
            {
                this.ModelState.AddModelError(
                    string.Empty,
                    string.Format(GlobalConstants.ForumNameAlreadyExistsErrorMessage, inputModel.Name));
            }

            if (!this.ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            return this.RedirectToAction(nameof(this.Index));
        }

        public IActionResult Edit(int id)
        {
            var forum = this.forumsService.GetById<ForumEditInputModel>(id);

            if (forum == null)
            {
                return this.NotFound();
            }

            var categories = this.categoriesService.GetAllByForumId<CategoryEditViewModel>(id).ToArray();

            var viewModel = new ForumAdministrationEditInputModel
            {
                Forum = forum,
                Categories = categories,
            };

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ForumAdministrationEditInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                var forum = this.forumsService.GetById<ForumEditInputModel>(input.Id);
                var categories = this.categoriesService.GetAllByForumId<CategoryEditViewModel>(input.Id).ToArray();

                input.Forum = forum;
                input.Categories = categories;

                return this.View(input);
            }

            var forumId = await this.forumsService.EditAsync(input.Id, input.Forum.Name, input.CategoryIds, input.AreSelected);

            if (forumId == -1)
            {
                return this.NotFound();
            }

            if (forumId == 0)
            {
                this.ModelState.AddModelError(
                    "Forum.Name",
                    string.Format(GlobalConstants.CategoryNameAlreadyExistsErrorMessage, input.Forum.Name));
            }

            if (!this.ModelState.IsValid)
            {
                var forum = this.forumsService.GetById<ForumEditInputModel>(input.Id);
                var categories = this.categoriesService.GetAllByForumId<CategoryEditViewModel>(input.Id).ToArray();

                input.Forum = forum;
                input.Categories = categories;

                return this.View(input);
            }

            return this.RedirectToAction(nameof(this.Index));
        }

        public IActionResult Delete(int id)
        {
            var viewModel = this.forumsService.GetById<ForumDeleteViewModel>(id);

            if (viewModel == null)
            {
                return this.NotFound();
            }

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(ForumDeleteViewModel input)
        {
            await this.forumsService.DeleteAsync(input.Id);

            return this.RedirectToAction(nameof(this.Index));
        }
    }
}