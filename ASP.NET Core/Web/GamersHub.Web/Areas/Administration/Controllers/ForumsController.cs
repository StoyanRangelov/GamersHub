using System.Threading.Tasks;
using GamersHub.Common;
using GamersHub.Services.Data;
using GamersHub.Web.ViewModels.Administration.Forums;
using Microsoft.AspNetCore.Mvc;

namespace GamersHub.Web.Areas.Administration.Controllers
{
    public class ForumsController : AdministrationController
    {
        private readonly IForumsService forumsService;

        public ForumsController(IForumsService forumsService)
        {
            this.forumsService = forumsService;
        }

        public IActionResult Index()
        {
            var forums = this.forumsService.GetAll<ForumAdministrationViewModel>();

            var viewModel = new ForumAdministrationIndexViewModel { Forums = forums};

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

        public IActionResult Delete(int id)
        {
            var viewModel = this.forumsService.GetById<ForumDeleteViewModel>(id);

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