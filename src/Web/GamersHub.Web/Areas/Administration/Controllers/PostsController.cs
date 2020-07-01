namespace GamersHub.Web.Areas.Administration.Controllers
{
    using System;

    using GamersHub.Services.Data.Posts;
    using GamersHub.Web.ViewModels;
    using GamersHub.Web.ViewModels.Administration.Posts;
    using Microsoft.AspNetCore.Mvc;

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

            var viewModel = new PostIndexViewModel { Posts = posts };

            var count = this.postsService.GetCount();

            var pagination = new PaginationViewModel();

            pagination.PagesCount = (int)Math.Ceiling((double)count / PostsPerPage);
            if (pagination.PagesCount == 0)
            {
                pagination.PagesCount = 1;
            }

            pagination.CurrentPage = id;

            viewModel.Pagination = pagination;

            return this.View(viewModel);
        }
    }
}
