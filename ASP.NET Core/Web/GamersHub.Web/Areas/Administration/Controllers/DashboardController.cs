namespace GamersHub.Web.Areas.Administration.Controllers
{
    using System;
    using System.Threading.Tasks;

    using GamersHub.Common;
    using GamersHub.Services.Data.Categories;
    using GamersHub.Services.Data.Forums;
    using GamersHub.Services.Data.Games;
    using GamersHub.Services.Data.Parties;
    using GamersHub.Services.Data.Posts;
    using GamersHub.Services.Data.Users;
    using GamersHub.Web.ViewModels.Administration.Dashboard;
    using GamersHub.Web.ViewModels.Administration.Users;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Distributed;
    using Newtonsoft.Json;

    public class DashboardController : AdministrationController
    {
        private readonly IForumsService forumsService;
        private readonly ICategoriesService categoriesService;
        private readonly IPostsService postsService;
        private readonly IUsersService usersService;
        private readonly IGamesService gamesService;
        private readonly IPartiesService partiesService;
        private readonly IDistributedCache distributedCache;

        public DashboardController(
            IForumsService forumsService,
            ICategoriesService categoriesService,
            IPostsService postsService,
            IUsersService usersService,
            IGamesService gamesService,
            IPartiesService partiesService,
            IDistributedCache distributedCache)
        {
            this.forumsService = forumsService;
            this.categoriesService = categoriesService;
            this.postsService = postsService;
            this.usersService = usersService;
            this.gamesService = gamesService;
            this.partiesService = partiesService;
            this.distributedCache = distributedCache;
        }

        public async Task<IActionResult> Index()
        {
            var data = await this.distributedCache.GetStringAsync("AdministrationDashboardViewModel");
            if (data == null)
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

                var administrationDashboardViewModel = new IndexViewModel
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
                data = JsonConvert.SerializeObject(administrationDashboardViewModel);
                await this.distributedCache.SetStringAsync(
                    "AdministrationDashboardViewModel",
                    data,
                    new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(3),
                    });
            }

            var viewModel = JsonConvert.DeserializeObject<IndexViewModel>(data);

            return this.View(viewModel);
        }
    }
}
