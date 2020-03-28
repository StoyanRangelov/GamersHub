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
        private readonly ICategoriesService categoriesService;
        private readonly IForumsService forumsService;
        private readonly UserManager<ApplicationUser> userManager;

        public PostsController(
            IPostsService postsService,
            ICategoriesService categoriesService,
            IForumsService forumsService,
            UserManager<ApplicationUser> userManager)
        {
            this.postsService = postsService;
            this.categoriesService = categoriesService;
            this.forumsService = forumsService;
            this.userManager = userManager;
        }

        public IActionResult ById(int id)
        {
            var viewModel = this.postsService.GetById<PostByIdViewModel>(id);

            if (viewModel == null)
            {
                return this.NotFound();
            }

            return this.View(viewModel);
        }

        public IActionResult Create()
        {
            var forums = this.forumsService.GetAll<ForumDropDownViewModel>();
            var categories = this.categoriesService.GetAll<CategoryDropDownViewModel>();

            var viewModel = new PostCreateInputModel
            {
                Forums = forums,
                Categories = categories,
            };

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(PostCreateInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            var currentUser = await this.userManager.GetUserAsync(this.User);
            var userId = await this.userManager.GetUserIdAsync(currentUser);

            var postId = await this.postsService.CreateAsync(
                inputModel.ForumId,
                inputModel.CategoryId,
                inputModel.Name,
                inputModel.Content,
                userId);

            return this.RedirectToAction(nameof(this.ById), new {id = postId});
        }

        public IActionResult Edit(int id)
        {
            var viewModel = this.postsService.GetById<PostEditViewModel>(id);

            if (viewModel == null)
            {
                return this.NotFound();
            }

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(PostEditViewModel input)
        {
            await this.postsService.Edit(input.Id, input.Name, input.Content);

            return this.RedirectToAction(nameof(this.ById), new { id = input.Id });
        }
    }
}