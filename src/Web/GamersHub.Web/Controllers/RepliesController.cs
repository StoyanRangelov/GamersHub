﻿using GamersHub.Web.ViewModels.Replies.InputModels;

namespace GamersHub.Web.Controllers
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using GamersHub.Common;
    using GamersHub.Services.Data.Posts;
    using GamersHub.Services.Data.Replies;
    using GamersHub.Web.ViewModels.Replies;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class RepliesController : BaseController
    {
        private readonly IRepliesService repliesService;
        private readonly IPostsService postsService;

        public RepliesController(
            IPostsService postsService,
            IRepliesService repliesService)
        {
            this.postsService = postsService;
            this.repliesService = repliesService;
        }

        public IActionResult Create(int id)
        {
            var post = this.postsService.GetById<ReplyPostViewModel>(id);

            if (post == null)
            {
                return this.NotFound();
            }

            var viewModel = new ReplyCreateViewModel
            {
                Post = post,
            };

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ReplyCreateViewModel input)
        {
            if (!this.ModelState.IsValid)
            {
                var post = this.postsService.GetById<ReplyPostViewModel>(input.PostId);

                input.Post = post;

                return this.View(input);
            }

            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            await this.repliesService.CreateAsync(input.PostId, userId, input.Content);

            this.TempData["InfoMessage"] = "Reply created successfully!";
            return this.RedirectToAction("ByName", "Posts", new { name = input.PostUrl });
        }

        public IActionResult Edit(int id)
        {
            var viewModel = this.repliesService.GetById<ReplyEditViewModel>(id);

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
        public async Task<IActionResult> Edit(ReplyEditViewModel input)
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
                var viewModel = this.repliesService.GetById<ReplyEditViewModel>(input.Id);

                return this.View(viewModel);
            }

            var replyId = await this.repliesService.EditAsync(input.Id, input.Content);

            if (replyId == null)
            {
                return this.NotFound();
            }

            this.TempData["InfoMessage"] = "Reply edited successfully!";
            return this.RedirectToAction("ByName", "Posts", new { name = input.PostUrl });
        }

        public IActionResult Delete(int id)
        {
            var viewModel = this.repliesService.GetById<ReplyDeleteViewModel>(id);

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
        public async Task<IActionResult> Delete(ReplyDeleteInputModel input)
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

            var replyId = await this.repliesService.DeleteAsync(input.ReplyId);
            if (replyId == null)
            {
                return this.NotFound();
            }

            this.TempData["InfoMessage"] = "Reply deleted successfully!";
            return this.RedirectToAction("ByName", "Posts", new {name = input.PostUrl});
        }
    }
}
