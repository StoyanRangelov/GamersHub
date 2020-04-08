using System;
using System.IO;
using System.Threading.Tasks;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using GamersHub.Services.Data.Games;
using GamersHub.Web.ViewModels.Administration.Games;
using Microsoft.AspNetCore.Mvc;

namespace GamersHub.Web.Areas.Administration.Controllers
{
    public class GamesController : AdministrationController
    {
        private readonly IGamesService gamesService;
        private readonly Cloudinary cloudinary;

        public GamesController(Cloudinary cloudinary, IGamesService gamesService)
        {
            this.cloudinary = cloudinary;
            this.gamesService = gamesService;
        }

        public IActionResult Index()
        {
            return this.View();
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

            await using var stream = input.Image.OpenReadStream();

            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(input.Title, stream),
                Format = "jpg",
            };

            var uploadResult = await cloudinary.UploadAsync(uploadParams);

            var publicId = uploadResult.PublicId + ".jpg";

            var imageUrl = cloudinary.Api.UrlImgUp.BuildUrl(publicId);

            await this.gamesService.CreateAsync(input.Title, input.SubTitle, input.Description, imageUrl);

            this.TempData["InfoMessage"] = "Game created successfully!";
            return this.RedirectToAction(nameof(this.Index));
        }
    }
}