using System.Linq;
using GamersHub.Services.Data.Categories;
using GamersHub.Services.Data.Forums;
using GamersHub.Services.Data.Posts;

namespace GamersHub.Web.Areas.Administration.Controllers
{
    using GamersHub.Services.Data;
    using GamersHub.Web.ViewModels.Administration.Dashboard;
    using Microsoft.AspNetCore.Mvc;

    public class DashboardController : AdministrationController
    {
        private readonly IForumsService forumsService;
        private readonly ICategoriesService categoriesService;
        private readonly IPostsService postsService;

        public DashboardController(
            IForumsService forumsService,
            ICategoriesService categoriesService,
            IPostsService postsService)
        {
            this.forumsService = forumsService;
            this.categoriesService = categoriesService;
            this.postsService = postsService;
        }

        public IActionResult Index()
        {
            var forums = this.forumsService.GetAll<ForumDashboardViewModel>()
                .OrderByDescending(x=>x.PostsCount)
                .Take(5);
            var categories = this.categoriesService.GetAll<CategoryDashboardViewModel>()
                .OrderByDescending(x=>x.PostsCount)
                .Take(5);
            var posts = this.postsService.GetAll<PostDashboardViewModel>()
                .OrderByDescending(x => x.RepliesCount)
                .Take(5);

            var viewModel = new IndexViewModel
            {
                Forums = forums,
                Categories = categories,
                Posts = posts,
            };

            return this.View(viewModel);
        }
    }
}