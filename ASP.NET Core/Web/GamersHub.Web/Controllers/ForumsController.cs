using System;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using GamersHub.Common;
using GamersHub.Services.Data;
using GamersHub.Web.ViewModels.Forums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GamersHub.Web.Controllers
{
    [Authorize]
    public class ForumsController : BaseController
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
            var forums = this.forumsService
                .GetAll<ForumViewModel>()
                .OrderByDescending(x => x.PostsCount)
                .ThenByDescending(x => x.CategoryNames.Length);

            var viewModel = new ForumIndexViewModel { Forums = forums};


            return this.View(viewModel);
        }

        public IActionResult ByName(string url, string id)
        {
            if (id != null)
            {
                string categoryName = this.categoriesService.GetNameByUrl(id);

                this.ViewData["CategoryName"] = categoryName;
            }

            var viewModel = this.forumsService.GetByUrl<ForumByNameViewModel>(url);

            return this.View(viewModel);
        }

        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public IActionResult Create()
        {
            return this.View();
        }

        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        [HttpPost]
        public async Task<IActionResult> Create(ForumCreateInputModel inputModel)
        {
            bool alreadyExists = this.forumsService.CheckIfExistsByName(inputModel.Name);

            if (alreadyExists)
            {
                this.ModelState.AddModelError(
                    string.Empty,
                    string.Format(GlobalConstants.ForumNameAlreadyExistsErrorMessage, inputModel.Name));
            }

            if (!this.ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            await this.forumsService.CreateAsync(inputModel.Name);

            return this.RedirectToAction(nameof(this.Index));
        }
    }
}