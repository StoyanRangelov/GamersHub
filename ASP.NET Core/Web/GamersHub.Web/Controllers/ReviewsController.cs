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
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IUsersService usersService;

        public ReviewsController(
            IGamesService gamesService,
            IReviewsService reviewsService,
            UserManager<ApplicationUser> userManager,
            IUsersService usersService)
        {
            this.gamesService = gamesService;
            this.reviewsService = reviewsService;
            this.userManager = userManager;
            this.usersService = usersService;
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

        public async Task<IActionResult> Edit(int id)
        {
            var viewModel = this.reviewsService.GetById<ReviewEditViewModel>(id);

            if (viewModel == null)
            {
                return this.NotFound();
            }

            var user = await this.userManager.GetUserAsync(this.User);

            bool isUserAllowedToEdit = await this.usersService.ValidateUserCanEditDeleteById(viewModel.UserId, user);

            if (isUserAllowedToEdit == false)
            {
                return this.Redirect("/Identity/Account/AccessDenied");
            }

            return this.View(viewModel);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(ReviewEditViewModel input)
        {
            var user = await this.userManager.GetUserAsync(this.User);

            bool isUserAllowedToEdit = await this.usersService.ValidateUserCanEditDeleteById(input.UserId, user);

            if (isUserAllowedToEdit == false)
            {
                return this.Redirect("/Identity/Account/AccessDenied");
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

            return this.RedirectToAction("ById", "Games", new {id = input.GameId, name = input.GameUrl});
        }

        public async Task<IActionResult> Delete(int id)
        {
            var viewModel = this.reviewsService.GetById<ReviewDeleteViewModel>(id);

            if (viewModel == null)
            {
                return this.NotFound();
            }

            var user = await this.userManager.GetUserAsync(this.User);

            bool isUserAllowedToDelete = await this.usersService.ValidateUserCanEditDeleteById(viewModel.UserId, user);

            if (isUserAllowedToDelete == false)
            {
                return this.Redirect("/Identity/Account/AccessDenied");
            }

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(ReviewDeleteViewModel input)
        {
            var user = await this.userManager.GetUserAsync(this.User);

            bool isUserAllowedToDelete = await this.usersService.ValidateUserCanEditDeleteById(input.UserId, user);

            if (isUserAllowedToDelete == false)
            {
                return this.Redirect("/Identity/Account/AccessDenied");
            }

            await this.reviewsService.DeleteAsync(input.Id);

            return this.RedirectToAction("ById", "Games", new {id = input.GameId, name = input.GameUrl});
        }
    }
}