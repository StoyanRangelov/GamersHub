using System;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using GamersHub.Common;
using GamersHub.Services.Data;
using GamersHub.Services.Data.Categories;
using GamersHub.Services.Data.Forums;
using GamersHub.Services.Data.Posts;
using GamersHub.Web.ViewModels;
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
        private const int PostsPerPage = 14;

        private readonly IForumsService forumsService;
        private readonly IPostsService postsService;

        public ForumsController(IForumsService forumsService, IPostsService postsService)
        {
            this.forumsService = forumsService;
            this.postsService = postsService;
        }

        public IActionResult Index(int id = 1)
        {
            var forums = this.forumsService
                .GetAll<ForumViewModel>(ForumsPerPage, (id - 1) * ForumsPerPage);

            var viewModel = new ForumIndexViewModel {Forums = forums};

            var count = this.forumsService.GetCount();

            var pagination = new PaginationViewModel();

            pagination.PagesCount = (int) Math.Ceiling((double) count / ForumsPerPage);
            if (pagination.PagesCount == 0)
            {
                pagination.PagesCount = 1;
            }

            pagination.CurrentPage = id;

            viewModel.Pagination = pagination;

            return this.View(viewModel);
        }

        public IActionResult ByName(string name, string searchString, string currentFilter, int id = 1)
        {
            var viewModel = this.forumsService.GetByNameUrl<ForumByNameViewModel>(name);

            if (viewModel == null)
            {
                return this.NotFound();
            }

            viewModel.CurrentPage = id;

            if (searchString != null)
            {
                viewModel.CurrentPage = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            viewModel.ForumPosts =
                this.postsService.GetAllByForumId<PostInForumViewModel>(viewModel.Id, PostsPerPage, (id - 1) * PostsPerPage, searchString);

            var count = this.postsService.GetCountByForumId(viewModel.Id, searchString);

            viewModel.PagesCount = (int) Math.Ceiling((double) count / PostsPerPage);
            if (viewModel.PagesCount == 0)
            {
                viewModel.PagesCount = 1;
            }

            return this.View(viewModel);
        }
    }
}