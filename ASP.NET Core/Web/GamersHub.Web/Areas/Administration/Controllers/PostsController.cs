using System;
using GamersHub.Services.Data.Posts;
using GamersHub.Web.ViewModels.Administration.Posts;
using Microsoft.AspNetCore.Mvc;

namespace GamersHub.Web.Areas.Administration.Controllers
{
    public class PostsController : AdministrationController
    {
        private const int PostsPerPage = 14;

        private readonly IPostsService postsService;

        public PostsController(IPostsService postsService)
        {
            this.postsService = postsService;
        }

        public IActionResult Index(int id = 1)
        {
            var posts = this.postsService.GetAll<PostViewModel>(PostsPerPage, (id - 1) * PostsPerPage);

            var viewModel = new PostIndexViewModel { Posts = posts};

            var count = this.postsService.GetCount();

            viewModel.PagesCount = (int) Math.Ceiling((double) count / PostsPerPage);
            if (viewModel.PagesCount == 0)
            {
                viewModel.PagesCount = 1;
            }

            viewModel.CurrentPage = id;


            return this.View(viewModel);
        }
    }
}