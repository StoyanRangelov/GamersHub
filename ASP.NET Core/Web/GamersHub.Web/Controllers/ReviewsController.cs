using System.Security.Claims;
using System.Threading.Tasks;
using GamersHub.Common;
using GamersHub.Data.Models;
using GamersHub.Services.Data.Games;
using GamersHub.Services.Data.Reviews;
using GamersHub.Services.Data.Users;
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

        public ReviewsController(
            IGamesService gamesService,
            IReviewsService reviewsService) 
        {
            this.gamesService = gamesService;
            this.reviewsService = reviewsService;
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

            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            await this.reviewsService.CreateAsync(input.GameId, input.IsPositive, input.Content, userId);

            var gameUrl = this.gamesService.GetUrl(input.GameId);

            this.TempData["InfoMessage"] = "Review created successfully!";
            return this.RedirectToAction("ByName", "Games", new { name = gameUrl});
        }

        public ActionResult Edit(int id)
        {
            var viewModel = this.reviewsService.GetById<ReviewEditViewModel>(id);

            if (viewModel == null)
            {
                return this.NotFound();
            }

            if (!this.User.IsInRole(GlobalConstants.AdministratorRoleName) &&
                !this.User.IsInRole(GlobalConstants.ModeratorRoleName))
            {
                var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (userId != viewModel.UserId)
                {
                    return this.BadRequest();
                }
            }

            return this.View(viewModel);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(ReviewEditViewModel input)
        {
            if (!this.User.IsInRole(GlobalConstants.AdministratorRoleName) &&
                !this.User.IsInRole(GlobalConstants.ModeratorRoleName))
            {
                var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (userId != input.UserId)
                {
                    return this.BadRequest();
                }
            }

            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            var reviewId = await this.reviewsService.EditAsync(input.Id, input.Content, input.IsPositive);

            if (reviewId == 0)
            {
                return this.NotFound();
            }

            this.TempData["InfoMessage"] = "Review edited successfully!";
            return this.RedirectToAction("ByName", "Games", new { name = input.GameUrl});
        }

        public IActionResult Delete(int id)
        {
            var viewModel = this.reviewsService.GetById<ReviewDeleteViewModel>(id);

            if (viewModel == null)
            {
                return this.NotFound();
            }

            if (!this.User.IsInRole(GlobalConstants.AdministratorRoleName) &&
                !this.User.IsInRole(GlobalConstants.ModeratorRoleName))
            {
                var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (userId != viewModel.UserId)
                {
                    return this.BadRequest();
                }
            }

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(ReviewDeleteInputModel input)
        {
            if (!this.User.IsInRole(GlobalConstants.AdministratorRoleName) &&
                !this.User.IsInRole(GlobalConstants.ModeratorRoleName))
            {
                var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (userId != input.UserId)
                {
                    return this.BadRequest();
                }
            }

            await this.reviewsService.DeleteAsync(input.ReviewId);

            this.TempData["InfoMessage"] = "Review deleted successfully!";
            return this.RedirectToAction("ByName", "Games", new { name = input.GameUrl});
        }
    }
}