using GamersHub.Services.Data;
using GamersHub.Web.ViewModels.Forums;
using Microsoft.AspNetCore.Mvc;

namespace GamersHub.Web.Controllers
{
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
    }
}
