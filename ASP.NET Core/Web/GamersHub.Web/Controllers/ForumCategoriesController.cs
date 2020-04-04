using System;
using System.Linq;
using System.Threading.Tasks;
using GamersHub.Common;
using GamersHub.Services.Data;
using GamersHub.Services.Data.Categories;
using GamersHub.Services.Data.ForumCategories;
using GamersHub.Services.Data.Posts;
using GamersHub.Web.Infrastructure;
using GamersHub.Web.ViewModels.Categories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GamersHub.Web.Controllers
{
    [Authorize]
    [ControllerName("Categories")]
    public class ForumCategoriesController : BaseController
    {
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

        public IActionResult ByName(string name, int id)
        {
            var normalisedName = this.categoriesService.GetNormalisedName(name);

            var forumCategory = this.forumCategoriesService.GetByNameAndForumId<ForumCategoryByNameViewModel>(normalisedName, id);
            var categoryPosts = this.postsService.GetAllByCategoryNameAndForumId<PostInCategoryViewModel>(normalisedName,id);

            if (forumCategory == null || categoryPosts == null)
            {
                return this.NotFound();
            }

            var viewModel = new ForumCategoryViewModel
            {
                ForumCategory = forumCategory,
                CategoryPosts = categoryPosts,
            };

            // ReSharper disable once Mvc.ViewNotResolved
            // ForumCategoriesController responds to Views/Categories due ControllerName Change
            return this.View(viewModel);
        }
    }
}