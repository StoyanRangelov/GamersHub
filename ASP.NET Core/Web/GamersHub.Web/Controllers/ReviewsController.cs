using System.Threading.Tasks;
using GamersHub.Data.Models;
using GamersHub.Services.Data.Games;
using GamersHub.Services.Data.Reviews;
using GamersHub.Web.ViewModels.Reviews;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GamersHub.Web.Controllers
{
    [Authorize]
    public class ReviewsController : BaseController
    {
        private readonly IGamesService gamesService;
        private readonly IReviewsService reviewsService;
        private readonly UserManager<ApplicationUser> userManager;

        public ReviewsController(
            IGamesService gamesService,
            IReviewsService reviewsService,
            UserManager<ApplicationUser> userManager)
        {
            this.gamesService = gamesService;
            this.reviewsService = reviewsService;
            this.userManager = userManager;
        }

        public IActionResult Create()
        {
            var games = this.gamesService.GetAll<GameDropDownViewModel>();

            var viewModel = new ReviewCreateInputModel
            {
                Games = games,
            };

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ReviewCreateInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                var games = this.gamesService.GetAll<GameDropDownViewModel>();

                input.Games = games;

                return this.View(input);
            }

            var currentUser = await this.userManager.GetUserAsync(this.User);
            var userId = await this.userManager.GetUserIdAsync(currentUser);

            await this.reviewsService.CreateAsync(input.GameId, input.IsPositive, input.Content, userId);

            var gameUrl = this.gamesService.GetUrl(input.GameId);

            return this.RedirectToAction("ById", "Games", new {id = input.GameId, name = gameUrl});
        }

    }
}