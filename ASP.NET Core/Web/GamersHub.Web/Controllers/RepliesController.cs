using System.Threading.Tasks;
using GamersHub.Data.Models;
using GamersHub.Services.Data;
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

        public RepliesController(IPostsService postsService, IRepliesService repliesService,
            UserManager<ApplicationUser> userManager)
        {
            this.postsService = postsService;
            this.repliesService = repliesService;
            this.userManager = userManager;
        }

        public IActionResult Create(int id)
        {
            var post = this.postsService.GetById<ReplyPostCreateViewModel>(id);

            var viewModel = new ReplyCreateInputModel
            {
                Post = post,
            };

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ReplyCreateInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            var currentUser = await this.userManager.GetUserAsync(this.User);
            var userId = await this.userManager.GetUserIdAsync(currentUser);

            await this.repliesService.CreateAsync(input.PostId, userId, input.Content);

            return this.RedirectToAction("ById", "Posts", new {id = input.PostId, name = input.PostUrl});
        }

        public IActionResult Edit(int id)
        {
            var viewModel = this.repliesService.GetById<ReplyInputModel>(id);

            return this.View(viewModel);
        }

        public IActionResult Delete(int id)
        {
            return this.View();
        }
    }
}