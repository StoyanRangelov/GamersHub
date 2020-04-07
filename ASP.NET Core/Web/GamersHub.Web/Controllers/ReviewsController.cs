using System.Threading.Tasks;
using GamersHub.Common;
using GamersHub.Data.Models;
using GamersHub.Services.Data.Games;
using GamersHub.Services.Data.Reviews;
using GamersHub.Web.ViewModels.Replies;
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

        public IActionResult Edit(int id)
        {
            var viewModel = this.reviewsService.GetById<ReviewEditViewModel>(id);

            if (viewModel == null)
            {
                return this.NotFound();
            }

            return this.View(viewModel);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(ReviewEditViewModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            var reviewId = await this.reviewsService.EditAsync(input.Id, input.Content, input.IsPositive);

            if (reviewId == 0)
            {
                return this.NotFound();
            }

            return this.RedirectToAction("ById", "Games", new {id = input.GameId, name = input.GameUrl});
        }

        public IActionResult Delete(int id)
        {
            var viewModel = this.reviewsService.GetById<ReviewDeleteViewModel>(id);

            if (viewModel == null)
            {
                return this.NotFound();
            }

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(ReviewDeleteViewModel input)
        {
            await this.reviewsService.DeleteAsync(input.Id);

            return this.RedirectToAction("ById", "Games", new {id = input.GameId, name = input.GameUrl});
        }
    }
}