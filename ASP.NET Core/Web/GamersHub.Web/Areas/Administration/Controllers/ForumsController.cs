using System.Linq;
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

            var viewModel = new EditInputModel
            {
                Forum = forum,
                Categories = categories,
            };

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            if (input.CategoriesInput == null)
            {
                var forumId = await this.forumsService.EditAsync(input.Id, input.Forum.Name);

                if (forumId == 0)
                {
                    return this.NotFound();
                }
            }
            else
            {
                var categoryIds = input.CategoriesInput.Select(x => x.Id).ToArray();
                var areSelected = input.CategoriesInput.Select(x => x.IsSelected).ToArray();

                var forumId = await this.forumsService.EditAsync(input.Id, input.Forum.Name, categoryIds, areSelected);

                if (forumId == 0)
                {
                    return this.NotFound();
                }
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