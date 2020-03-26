using System.Threading.Tasks;
using GamersHub.Common;
using GamersHub.Data.Models;
using GamersHub.Services.Data;
using GamersHub.Web.ViewModels.Posts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GamersHub.Web.Controllers
{
    [Authorize]
    public class PostsController : BaseController
    {
        private readonly IPostsService postsService;
        private readonly UserManager<ApplicationUser> userManager;

        public PostsController(IPostsService postsService, UserManager<ApplicationUser> userManager)
        {
            this.postsService = postsService;
            this.userManager = userManager;
        }

        public IActionResult Details(int id)
        {
            var viewModel = this.postsService.GetById<PostDetailsViewModel>(id);

            return this.View(viewModel);
        }

        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreatePostInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            var currentUser = await this.userManager.GetUserAsync(this.User);
            var userId = await this.userManager.GetUserIdAsync(currentUser);

            await this.postsService.CreateAsync(
                inputModel.ForumName,
                inputModel.CategoryName,
                inputModel.Name,
                inputModel.Content,
                userId);

            var url = UrlParser.ParseToUrl(inputModel.ForumName);

            return this.RedirectToAction("ByName", "Forums",  new { url });
        }
    }
}