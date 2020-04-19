using System.Threading.Tasks;
using GamersHub.Common;
using GamersHub.Services.Data.Pages;
using GamersHub.Web.ViewModels.Pages;
using Microsoft.AspNetCore.Authorization;

namespace GamersHub.Web.Controllers
{
    using System.Diagnostics;
    using GamersHub.Web.ViewModels;
    using Microsoft.AspNetCore.Mvc;

    public class HomeController : BaseController
    {
        private readonly IPagesService pagesService;

        public HomeController(IPagesService pagesService)
        {
            this.pagesService = pagesService;
        }

        public IActionResult Index()
        {
            return this.View();
        }

        public IActionResult Privacy()
        {
            var viewModel = this.pagesService.GetByName<PrivacyPageViewModel>(nameof(this.Privacy));
            if (viewModel == null)
            {
                return this.NotFound();
            }

            return this.View(viewModel);
        }

        [Authorize(Roles = GlobalConstants.AdministratorAndModeratorRoleNames)]
        public IActionResult EditPrivacy()
        {
            var viewModel = this.pagesService.GetByName<PrivacyPageEditInputModel>(nameof(this.Privacy));
            if (viewModel == null)
            {
                return this.NotFound();
            }

            return this.View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = GlobalConstants.AdministratorAndModeratorRoleNames)]
        public async Task<IActionResult> EditPrivacy(PrivacyPageEditInputModel inputModel)
        {
            var pageId = await this.pagesService.EditAsync(nameof(this.Privacy), inputModel.Content);
            if (pageId == null)
            {
                return this.NotFound();
            }

            this.TempData["Info Message"] = "Successfully edited Privacy Page";
            return this.RedirectToAction(nameof(this.Privacy));
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return this.View(
                new ErrorViewModel {RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier});
        }
    }
}