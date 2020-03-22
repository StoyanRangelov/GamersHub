using GamersHub.Services.Data;
using Microsoft.AspNetCore.Mvc;

namespace GamersHub.Web.Infrastructure
{
    public class CategoryNamesViewComponent : ViewComponent
    {
        private readonly ICategoriesService categoriesService;

        public CategoryNamesViewComponent(ICategoriesService categoriesService)
        {
            this.categoriesService = categoriesService;
        }

        public IViewComponentResult Invoke()
        {
            var forumNames = this.categoriesService.GetAllNames();

            return this.View(forumNames);
        }
    }
}