using System.Security.Claims;
using System.Threading.Tasks;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using GamersHub.Common;
using GamersHub.Services.Data.Games;
using GamersHub.Web.ViewModels.Games;
using GamersHub.Web.ViewModels.Posts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GamersHub.Web.Controllers
{
    [Authorize]
    public class GamesController : BaseController
    {
        private readonly IGamesService gamesService;
        private readonly Cloudinary cloudinary;

        public GamesController(IGamesService gamesService, Cloudinary cloudinary)
        {
            this.gamesService = gamesService;
            this.cloudinary = cloudinary;
        }

        public IActionResult Index()
        {
            var games = this.gamesService.GetAll<GameViewModel>();

            var viewModel = new GameIndexViewModel {Games = games};

            return this.View(viewModel);
        }

        public IActionResult ById(int id)
        {
            var viewModel = this.gamesService.GetById<GameByIdViewModel>(id);

            if (viewModel == null)
            {
                return this.NotFound();
            }

            return this.View(viewModel);
        }

        [Authorize(Roles = GlobalConstants.AdministratorAndModeratorRoleNames)]
        public IActionResult Edit(int id)
        {
            var viewModel = this.gamesService.GetById<GameEditViewModel>(id);

            if (viewModel == null)
            {
                return this.NotFound();
            }

            return this.View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = GlobalConstants.AdministratorAndModeratorRoleNames)]
        public async Task<IActionResult> Edit(GameEditViewModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            var imageUrl = string.Empty;
            if (input.Image != null)
            {
                await using var stream = input.Image.OpenReadStream();

                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(input.Title, stream),
                    Format = "jpg",
                };

                var uploadResult = await cloudinary.UploadAsync(uploadParams);

                var publicId = uploadResult.PublicId + ".jpg";

                imageUrl = cloudinary.Api.UrlImgUp.BuildUrl(publicId);
            }

            var gameId = await this.gamesService.EditAsync(
                input.Id,
                input.Title,
                input.SubTitle,
                input.Description,
                imageUrl);

            if (gameId == 0)
            {
                return this.NotFound();
            }

            this.TempData["InfoMessage"] = "Game edited successfully!";
            return this.RedirectToAction(nameof(this.ById), new {id = input.Id, name = input.Url});
        }
    }
}