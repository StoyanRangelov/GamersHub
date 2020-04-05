using System.Linq;
using GamersHub.Data.Models;
using GamersHub.Services.Data.Categories;
using GamersHub.Services.Data.Forums;
using GamersHub.Services.Data.Posts;
using GamersHub.Services.Data.Users;
using GamersHub.Services.Mapping;
using GamersHub.Web.ViewModels.Administration.Users;
using Microsoft.AspNetCore.Identity;

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
        private readonly IUsersService usersService;

        public DashboardController(
            IForumsService forumsService,
            ICategoriesService categoriesService,
            IPostsService postsService,
            IUsersService usersService)
        {
            this.forumsService = forumsService;
            this.categoriesService = categoriesService;
            this.postsService = postsService;
            this.usersService = usersService;
        }

        public IActionResult Index()
        {
            var forums = this.forumsService.GetTopFive<ForumDashboardViewModel>();

            var categories = this.categoriesService.GetTopFive<CategoryDashboardViewModel>();

            var posts = this.postsService.GetTopFive<PostDashboardViewModel>();

            var users = this.usersService.GetTopFive<UserDashboardViewModel>();

            var administrators = this.usersService.GetAllAdministrators<UserInRoleViewModel>();

            var moderators = this.usersService.GetAllModerators<UserInRoleViewModel>();

            var viewModel = new IndexViewModel
            {
                Forums = forums,
                Categories = categories,
                Posts = posts,
                Users = users,
                Administrators = administrators,
                Moderators = moderators,
            };

            return this.View(viewModel);
        }
    }
}