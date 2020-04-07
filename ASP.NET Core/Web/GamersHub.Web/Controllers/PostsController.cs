using System.Threading.Tasks;
using GamersHub.Common;
using GamersHub.Data.Models;
using GamersHub.Services.Data;
using GamersHub.Services.Data.Categories;
using GamersHub.Services.Data.Forums;
using GamersHub.Services.Data.Posts;
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
                var forums = this.forumsService.GetAll<ForumDropDownViewModel>();
                var categories = this.categoriesService.GetAll<CategoryDropDownViewModel>();

                inputModel.Forums = forums;
                inputModel.Categories = categories;

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

            return this.RedirectToAction(nameof(this.ById), new {id = postId, name = inputModel.Url});
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
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            var postId = await this.postsService.EditAsync(input.Id, input.Name, input.Content);

            if (postId == 0)
            {
                return this.NotFound();
            }

            return this.RedirectToAction(nameof(this.ById), new {id = input.Id, name = input.Url});
        }


        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public IActionResult Delete(int id)
        {
            var viedModel = this.postsService.GetById<PostDeleteViewModel>(id);

            return this.View(viedModel);
        }

        [HttpPost]
        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public async Task<IActionResult> Delete(PostDeleteViewModel input)
        {
            await this.postsService.DeleteAsync(input.Id);

            var forumUrl = UrlParser.ParseToUrl(input.ForumName);

            return this.RedirectToAction("ById", "Forums", new {id = input.ForumId, name = forumUrl});
        }
    }
}
