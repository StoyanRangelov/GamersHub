using System;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using GamersHub.Common;
using GamersHub.Services.Data;
using GamersHub.Services.Data.Categories;
using GamersHub.Services.Data.Forums;
using GamersHub.Web.ViewModels.Administration.Forums;
using GamersHub.Web.ViewModels.Forums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GamersHub.Web.Controllers
{
    [Authorize]
    public class ForumsController : BaseController
    {
        private const int ForumsPerPage = 6;

        private readonly IForumsService forumsService;

        public ForumsController(IForumsService forumsService)
        {
            this.forumsService = forumsService;
        }

        public IActionResult Index(int id = 1)
        {
            var forums = this.forumsService
                .GetAll<ForumViewModel>(ForumsPerPage, (id - 1) * ForumsPerPage);

            var viewModel = new ForumIndexViewModel {Forums = forums};

            var count = this.forumsService.GetCount();

            viewModel.PagesCount = (int) Math.Ceiling((double) count / ForumsPerPage);
            if (viewModel.PagesCount == 0)
            {
                viewModel.PagesCount = 1;
            }

            viewModel.CurrentPage = id;

            return this.View(viewModel);
        }

        public IActionResult ById(int id)
        {
            var viewModel = this.forumsService.GetById<ForumByIdViewModel>(id);

            if (viewModel == null)
            {
                return this.NotFound();
            }

            return this.View(viewModel);
        }
    }
}