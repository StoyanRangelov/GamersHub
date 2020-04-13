using System.Linq;
using GamersHub.Common;
using GamersHub.Data.Models;
using GamersHub.Services.Data.Categories;
using GamersHub.Services.Data.Forums;
using GamersHub.Services.Data.Games;
using GamersHub.Services.Data.Parties;
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
        private readonly IGamesService gamesService;
        private readonly IPartiesService partiesService;

        public DashboardController(
            IForumsService forumsService,
            ICategoriesService categoriesService,
            IPostsService postsService,
            IUsersService usersService,
            IGamesService gamesService,
            IPartiesService partiesService)
        {
            this.forumsService = forumsService;
            this.categoriesService = categoriesService;
            this.postsService = postsService;
            this.usersService = usersService;
            this.gamesService = gamesService;
            this.partiesService = partiesService;
        }

        public IActionResult Index()
        {
            var games = this.gamesService
                .GetAll<GameDashboardViewModel>(5);

            var forums = this.forumsService
                .GetAll<ForumDashboardViewModel>(5);

            var categories = this.categoriesService
                .GetAll<CategoryDashboardViewModel>(5);

            var posts = this.postsService
                .GetTopFive<PostDashboardViewModel>();

            var parties = this.partiesService
                .GetTopFive<PartyDashboardViewModel>();

            var forumUsers = this.usersService
                .GetTopFive<ForumUserDashboardViewModel>(GlobalConstants.Posts);

            var gameUsers = this.usersService
                .GetTopFive<GameUserDashboardViewModel>(GlobalConstants.Reviews);

            var bannedUsers = this.usersService
                .GetTopFive<UserBannedDashboardViewModel>(GlobalConstants.Banned);

            var administrators = this.usersService
                .GetAllByRole<UserInRoleViewModel>(GlobalConstants.AdministratorRoleName);

            var moderators = this.usersService
                .GetAllByRole<UserInRoleViewModel>(GlobalConstants.ModeratorRoleName);

            var viewModel = new IndexViewModel
            {
                Games = games,
                Forums = forums,
                Categories = categories,
                Posts = posts,
                Parties = parties,
                ForumUsers = forumUsers,
                GameUsers = gameUsers,
                BannedUsers = bannedUsers,
                Administrators = administrators,
                Moderators = moderators,
            };

            return this.View(viewModel);
        }
    }
}