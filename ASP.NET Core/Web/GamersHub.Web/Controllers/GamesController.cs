﻿using System;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using GamersHub.Common;
using GamersHub.Services.Data.Games;
using GamersHub.Services.Data.Reviews;
using GamersHub.Web.ViewModels;
using GamersHub.Web.ViewModels.Games;
using GamersHub.Web.ViewModels.Posts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GamersHub.Web.Controllers
{
    [Authorize]
    public class GamesController : BaseController
    {
        private const int GamesPerPage = 4;
        private const int ReviewsPerPage = 12;

        private readonly IGamesService gamesService;
        private readonly IReviewsService reviewsService;
        private readonly Cloudinary cloudinary;

        public GamesController(IGamesService gamesService, Cloudinary cloudinary, IReviewsService reviewsService)
        {
            this.gamesService = gamesService;
            this.cloudinary = cloudinary;
            this.reviewsService = reviewsService;
        }

        public IActionResult Index(int id = 1)
        {
            var games = this.gamesService
                .GetAll<GameViewModel>(GamesPerPage, (id - 1) * GamesPerPage);

            var viewModel = new GameIndexViewModel {Games = games};

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

        public IActionResult ByName(string name, int id = 1)
        {
            var viewModel = this.gamesService.GetByName<GameByIdViewModel>(name);

            if (viewModel == null)
            {
                return this.NotFound();
            }

            viewModel.GameReviews = this.reviewsService
                .GetAllByGameId<ReviewInGameViewModel>(viewModel.Id, ReviewsPerPage, (id - 1) * ReviewsPerPage);

            var count = this.reviewsService.GetCountByGameId(viewModel.Id);

            var pagination = new PaginationViewModel();

            pagination.PagesCount = (int)Math.Ceiling((double) count / ReviewsPerPage);
            if (pagination.PagesCount == 0)
            {
                pagination.PagesCount = 1;
            }

            pagination.CurrentPage = id;

            viewModel.Pagination = pagination;

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
                var fileName = ContentDispositionHeaderValue.Parse(input.Image.ContentDisposition).FileName.Trim('"');

                await using (var stream = input.Image.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams
                    {
                        File = new FileDescription(fileName, stream),
                        Format = "jpg",
                    };

                    var uploadResult = await cloudinary.UploadAsync(uploadParams);

                    imageUrl = uploadResult.SecureUri.ToString();
                }
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
            return this.RedirectToAction(nameof(this.ByName), new { name = input.Url});
        }
    }
}