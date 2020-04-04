using GamersHub.Services.Data.Posts;
using GamersHub.Web.ViewModels.Administration.Posts;
using Microsoft.AspNetCore.Mvc;

namespace GamersHub.Web.Areas.Administration.Controllers
{
    public class PostsController : AdministrationController
    {
        private readonly IPostsService postsService;

        public PostsController(IPostsService postsService)
        {
            this.postsService = postsService;
        }

        public IActionResult Index()
        {
            var posts = this.postsService.GetAll<PostViewModel>();

            var viewModel = new PostIndexViewModel { Posts = posts};

            return this.View(viewModel);
        }
    }
}