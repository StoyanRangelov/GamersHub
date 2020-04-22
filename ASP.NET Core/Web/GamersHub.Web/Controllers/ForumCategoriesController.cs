using System;
using System.Linq;
using System.Threading.Tasks;
using GamersHub.Common;
using GamersHub.Services.Data;
using GamersHub.Services.Data.Categories;
using GamersHub.Services.Data.ForumCategories;
using GamersHub.Services.Data.Posts;
using GamersHub.Web.Infrastructure;
using GamersHub.Web.ViewModels;
using GamersHub.Web.ViewModels.Categories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GamersHub.Web.Controllers
{
    [Authorize]
    [ControllerName("Categories")]
    public class ForumCategoriesController : BaseController
    {
        private const int PostPerPage = 14;

        private readonly IForumCategoriesService forumCategoriesService;
        private readonly ICategoriesService categoriesService;
        private readonly IPostsService postsService;

        public ForumCategoriesController(
            IForumCategoriesService forumCategoriesService,
            IPostsService postsService,
            ICategoriesService categoriesService)
        {
            this.forumCategoriesService = forumCategoriesService;
            this.postsService = postsService;
            this.categoriesService = categoriesService;
        }

        public IActionResult ByName(string name, int id, string searchString, string currentFilter, int page = 1)
        {
            var normalisedName = this.categoriesService.GetNormalisedName(name);

            var forumCategory =
                this.forumCategoriesService.GetByCategoryNameAndForumId<ForumCategoryByNameViewModel>(normalisedName, id);

            if (forumCategory == null)
            {
                return this.NotFound();
            }

            var viewModel = new ForumCategoryViewModel();
            viewModel.ForumCategory = forumCategory;

            viewModel.CurrentPage = page;

            if (searchString != null)
            {
                viewModel.CurrentPage = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            viewModel.CategoryPosts = this.postsService
                .GetAllByCategoryNameAndForumId<PostInCategoryViewModel>(normalisedName, id, PostPerPage,
                    (page - 1) * PostPerPage, searchString);


            var count = this.postsService.GetCountByCategoryNameAndForumId(normalisedName, id, searchString);

            viewModel.PagesCount = (int) Math.Ceiling((double) count / PostPerPage);
            if (viewModel.PagesCount == 0)
            {
                viewModel.PagesCount = 1;
            }

            // ReSharper disable once Mvc.ViewNotResolved
            // ForumCategoriesController responds to Views/Categories due ControllerName Change
            return this.View(viewModel);
        }
    }
}