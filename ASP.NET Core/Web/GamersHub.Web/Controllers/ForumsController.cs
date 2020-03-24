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

        public ForumsController(IForumsService forumsService)
        {
            this.forumsService = forumsService;
        }

        public IActionResult Index()
        {
            var viewModel = new ForumIndexViewModel
            {
                Forums = this.forumsService.GetAll<ForumViewModel>(),
            };

            return this.View(viewModel);
        }

        public IActionResult ByName(string name, string id)
        {
            if (id != null)
            {
                string categoryName = id.Replace('-', ' ');

                this.ViewData["CategoryName"] = categoryName;
            }

            var viewModel = this.forumsService.GetByName<ForumByNameViewModel>(name);

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
                this.ModelState.AddModelError(string.Empty,
                    string.Format(GlobalConstants.ForumNameAlreadyExistsErrorMessage, inputModel.Name));
            }

            if (!this.ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            await this.forumsService.CreateAsync(inputModel.Name);

            return this.Redirect("/Forums");
        }
    }
}