using System.Security.Claims;
using System.Threading.Tasks;
using GamersHub.Common;
using GamersHub.Data.Models;
using GamersHub.Services.Data;
using GamersHub.Services.Data.Categories;
using GamersHub.Services.Data.Forums;
using GamersHub.Services.Data.Posts;
using GamersHub.Services.Data.Users;
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

        public PostsController(
            IPostsService postsService,
            ICategoriesService categoriesService,
            IForumsService forumsService)
        {
            this.postsService = postsService;
            this.categoriesService = categoriesService;
            this.forumsService = forumsService;
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

            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var postId = await this.postsService.CreateAsync(
                inputModel.ForumId,
                inputModel.CategoryId,
                inputModel.Name,
                inputModel.Content,
                userId);

            if (postId == 0)
            {
                return this.NotFound();
            }

            this.TempData["InfoMessage"] = "Post created successfully!";
            return this.RedirectToAction(nameof(this.ById), new {id = postId, name = inputModel.Url});
        }

        public IActionResult Edit(int id)
        {
            var viewModel = this.postsService.GetById<PostEditViewModel>(id);

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
                    return this.Redirect("/Identity/Account/AccessDenied");
                }
            }

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(PostEditViewModel input)
        {
            if (!this.User.IsInRole(GlobalConstants.AdministratorRoleName) &&
                !this.User.IsInRole(GlobalConstants.ModeratorRoleName))
            {
                var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (userId != input.UserId)
                {
                    return this.Redirect("/Identity/Account/AccessDenied");
                }
            }

            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            var postId = await this.postsService.EditAsync(input.Id, input.Name, input.Content);

            if (postId == 0)
            {
                return this.NotFound();
            }

            this.TempData["InfoMessage"] = "Post edited successfully!";
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

            this.TempData["InfoMessage"] = "Post deleted successfully!";
            return this.RedirectToAction("ByName", "Forums", new {name = forumUrl});
        }
    }
}