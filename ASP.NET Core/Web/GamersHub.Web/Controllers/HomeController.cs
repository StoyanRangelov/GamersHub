using System;
using System.Threading.Tasks;
using GamersHub.Common;
using GamersHub.Services.Data.Forums;
using GamersHub.Services.Data.Games;
using GamersHub.Services.Data.Pages;
using GamersHub.Services.Data.Parties;
using GamersHub.Services.Data.Posts;
using GamersHub.Services.Data.Users;
using GamersHub.Web.ViewModels.Home;
using GamersHub.Web.ViewModels.Pages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace GamersHub.Web.Controllers
{
    using System.Diagnostics;
    using GamersHub.Web.ViewModels;
    using Microsoft.AspNetCore.Mvc;

    public class HomeController : BaseController
    {
        private readonly IPagesService pagesService;
        private readonly IGamesService gamesService;
        private readonly IPostsService postsService;
        private readonly IPartiesService partiesService;
        private readonly IUsersService usersService;
        private readonly IDistributedCache distributedCache;

        public HomeController(
            IPagesService pagesService,
            IGamesService gamesService,
            IPostsService postsService,
            IPartiesService partiesService,
            IUsersService usersService,
            IDistributedCache distributedCache)
        {
            this.pagesService = pagesService;
            this.gamesService = gamesService;
            this.postsService = postsService;
            this.partiesService = partiesService;
            this.usersService = usersService;
            this.distributedCache = distributedCache;
        }

        public async Task<IActionResult> Index()
        {
            var data = await this.distributedCache.GetStringAsync("HomeIndexViewModel");
            if (data == null)
            {
                var games = this.gamesService.GetAll<GameHomeIndexViewModel>(5);
                var posts = this.postsService.GetTopFive<PostHomeIndexViewModel>();
                var parties = this.partiesService.GetTopFive<PartyHomeIndexViewModel>();
                var users = this.usersService.GetTopFive<UserHomeIndexViewModel>();
                var homeIndexViewModel = new HomeIndexViewModel
                {
                    Games = games,
                    Posts = posts,
                    Parties = parties,
                    TopUsers = users,
                };
                data = JsonConvert.SerializeObject(homeIndexViewModel);
                await this.distributedCache.SetStringAsync("HomeIndexViewModel", data, new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5),
                });
            }

            var viewModel = JsonConvert.DeserializeObject<HomeIndexViewModel>(data);

            return this.View(viewModel);
        }

        public IActionResult Chat(string id)
        {
            this.ViewData["ChatId"] = id;
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

        public IActionResult HttpError(int statusCode)
        {
            return this.View(new HttpErrorViewModel
                {StatusCode = statusCode, Message = GlobalConstants.HttpErrorMessage});
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return this.View(
                new ErrorViewModel {RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier});
        }
    }
}