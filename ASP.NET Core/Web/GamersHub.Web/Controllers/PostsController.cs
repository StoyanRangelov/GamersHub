using GamersHub.Services.Data;
using GamersHub.Web.ViewModels.Posts;
using Microsoft.AspNetCore.Mvc;

namespace GamersHub.Web.Controllers
{
    public class PostsController : BaseController
    {
        private readonly IForumsService forumsService;
        private readonly IPostsService postsService;

        public PostsController(IForumsService forumsService, IPostsService postsService)
        {
            this.forumsService = forumsService;
            this.postsService = postsService;
        }

        public IActionResult Create(int id)
        {
            var forum = this.forumsService.GetById<CreatePostViewModel>(id);

            this.ViewData["Name"] = forum.Name;
            this.ViewBag.CategoryNames = forum.CategoryNames;

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
                inputModel.Topic,
                inputModel.Content,
                this.User.Identity.Name);

            var url = inputModel.ForumName.Replace(' ', '-');

            return this.Redirect($"/Forums/{url}");
        }
    }
}
