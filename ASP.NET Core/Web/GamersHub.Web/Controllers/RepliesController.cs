using System.Threading.Tasks;
using GamersHub.Data.Models;
using GamersHub.Services.Data;
using GamersHub.Services.Data.Posts;
using GamersHub.Services.Data.Replies;
using GamersHub.Web.ViewModels.Posts;
using GamersHub.Web.ViewModels.Replies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GamersHub.Web.Controllers
{
    [Authorize]
    public class RepliesController : BaseController
    {
        private readonly IRepliesService repliesService;
        private readonly IPostsService postsService;
        private readonly UserManager<ApplicationUser> userManager;

        public RepliesController(
            IPostsService postsService,
            IRepliesService repliesService,
            UserManager<ApplicationUser> userManager)
        {
            this.postsService = postsService;
            this.repliesService = repliesService;
            this.userManager = userManager;
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

            var currentUser = await this.userManager.GetUserAsync(this.User);
            var userId = await this.userManager.GetUserIdAsync(currentUser);

            await this.repliesService.CreateAsync(input.PostId, userId, input.Content);

            return this.RedirectToAction("ById", "Posts", new {id = input.PostId, name = input.PostUrl});
        }

        public IActionResult Edit(int id)
        {
            var viewModel = this.repliesService.GetById<ReplyEditViewModel>(id);

            if (viewModel == null)
            {
                return this.NotFound();
            }

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ReplyEditViewModel input)
        {
            if (!this.ModelState.IsValid)
            {
                var viewModel = this.repliesService.GetById<ReplyEditViewModel>(input.Id);

                return this.View(viewModel);
            }

            var replyId = await this.repliesService.EditAsync(input.Id, input.Content);

            if (replyId == 0)
            {
                return this.NotFound();
            }

            return this.RedirectToAction("ById", "Posts", new {id = input.PostId, name = input.PostUrl});

        }

        public IActionResult Delete(int id)
        {
            var viewModel = this.repliesService.GetById<ReplyDeleteViewModel>(id);

            if (viewModel == null)
            {
                return this.NotFound();
            }

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(ReplyDeleteViewModel input)
        {
           await this.repliesService.DeleteAsync(input.Id);

           return this.RedirectToAction("ById", "Posts", new {id = input.PostId, name = input.PostUrl});
        }
    }
}
