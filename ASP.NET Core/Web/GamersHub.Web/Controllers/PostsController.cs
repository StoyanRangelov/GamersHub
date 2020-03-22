using GamersHub.Services.Data;
using GamersHub.Web.ViewModels.Posts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GamersHub.Web.Controllers
{
    [Authorize]
    public class PostsController : BaseController
    {
        private readonly IForumsService forumsService;
        private readonly IPostsService postsService;

        public PostsController(IForumsService forumsService, IPostsService postsService)
        {
            this.forumsService = forumsService;
            this.postsService = postsService;
        }

        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        public IActionResult Create(CreatePostInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            this.postsService.Create(
                inputModel.ForumName,
                inputModel.CategoryName,
                inputModel.Name,
                inputModel.Content,
                this.User.Identity.Name);

            var url = inputModel.ForumName.Replace(' ', '-');

            return this.Redirect($"/Forums/{url}");
        }
    }
}
