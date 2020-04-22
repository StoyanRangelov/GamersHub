using System;
using System.IO;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using GamersHub.Services.Data.Games;
using GamersHub.Web.ViewModels;
using GamersHub.Web.ViewModels.Administration.Games;
using Microsoft.AspNetCore.Mvc;

namespace GamersHub.Web.Areas.Administration.Controllers
{
    public class GamesController : AdministrationController
    {
        private const int GamesPerPage = 14;

        private readonly IGamesService gamesService;
        private readonly Cloudinary cloudinary;

        public GamesController(Cloudinary cloudinary, IGamesService gamesService)
        {
            this.cloudinary = cloudinary;
            this.gamesService = gamesService;
        }

        public IActionResult Index(int id = 1)
        {
            var games = this.gamesService
                .GetAll<GameAdministrationViewModel>(GamesPerPage,(id - 1) * GamesPerPage);

            var viewModel = new GameAdministrationIndexViewModel {Games = games};

            var count = this.gamesService.GetCount();

            var pagination = new PaginationViewModel();
            pagination.PagesCount = (int) Math.Ceiling((double) count / GamesPerPage);
            if (pagination.PagesCount == 0)
            {
                pagination.PagesCount = 1;
            }

            pagination.CurrentPage = id;

            viewModel.Pagination = pagination;

            return this.View(viewModel);
        }

        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(GameCreateInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            var fileName = ContentDispositionHeaderValue.Parse(input.Image.ContentDisposition).FileName.Trim('"');

            await using var stream = input.Image.OpenReadStream();

            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(fileName, stream),
                Format = "jpg",
            };

            var uploadResult = await cloudinary.UploadAsync(uploadParams);

            var imageUrl = uploadResult.SecureUri.ToString();

            await this.gamesService.CreateAsync(input.Title, input.SubTitle, input.Description, imageUrl);

            this.TempData["InfoMessage"] = "Game created successfully!";
            return this.RedirectToAction(nameof(this.Index));
        }

        public IActionResult Delete(int id)
        {
            var viewModel = this.gamesService.GetById<GameDeleteViewModel>(id);

            if (viewModel == null)
            {
                return this.NotFound();
            }

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(GameDeleteViewModel input)
        {
            await this.gamesService.DeleteAsync(input.Id);

            this.TempData["InfoMessage"] = "Game deleted successfully!";
            return this.RedirectToAction(nameof(this.Index));
        }
    }
}