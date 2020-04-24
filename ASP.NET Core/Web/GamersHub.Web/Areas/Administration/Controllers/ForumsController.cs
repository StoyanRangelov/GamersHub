using GamersHub.Web.ViewModels.Administration.Forums.InputModels;

namespace GamersHub.Web.Areas.Administration.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using GamersHub.Common;
    using GamersHub.Services.Data.Categories;
    using GamersHub.Services.Data.Forums;
    using GamersHub.Web.ViewModels;
    using GamersHub.Web.ViewModels.Administration.Forums;
    using Microsoft.AspNetCore.Mvc;

    public class ForumsController : AdministrationController
    {
        private const int ForumsPerPage = 14;

        private readonly IForumsService forumsService;
        private readonly ICategoriesService categoriesService;

        public ForumsController(IForumsService forumsService, ICategoriesService categoriesService)
        {
            this.forumsService = forumsService;
            this.categoriesService = categoriesService;
        }

        public IActionResult Index(int id = 1)
        {
            var forums = this.forumsService
                .GetAll<ForumAdministrationViewModel>(ForumsPerPage, (id - 1) * ForumsPerPage);

            var viewModel = new ForumAdministrationIndexViewModel { Forums = forums };

            var count = this.forumsService.GetCount();

            var pagination = new PaginationViewModel();

            pagination.PagesCount = (int)Math.Ceiling((double)count / ForumsPerPage);
            if (pagination.PagesCount == 0)
            {
                pagination.PagesCount = 1;
            }

            pagination.CurrentPage = id;

            viewModel.Pagination = pagination;

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

            this.TempData["InfoMessage"] = "Forum created successfully!";
            return this.RedirectToAction(nameof(this.Index));
        }

        public IActionResult Edit(int id)
        {
            var forum = this.forumsService.GetById<ForumEditInputModel>(id);

            if (forum == null)
            {
                return this.NotFound();
            }

            var categories = this.categoriesService.GetAllMissingByForumId<CategoryEditViewModel>(id).ToArray();

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
                var categories = this.categoriesService.GetAllMissingByForumId<CategoryEditViewModel>(input.Id).ToArray();

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
                var categories = this.categoriesService.GetAllMissingByForumId<CategoryEditViewModel>(input.Id).ToArray();

                input.Forum = forum;
                input.Categories = categories;

                return this.View(input);
            }

            this.TempData["InfoMessage"] = "Forum edited successfully!";
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

            this.TempData["InfoMessage"] = "Forum deleted successfully!";
            return this.RedirectToAction(nameof(this.Index));
        }
    }
}
