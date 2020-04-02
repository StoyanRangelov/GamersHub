using System.Linq;

namespace GamersHub.Web.Areas.Administration.Controllers
{
    using GamersHub.Services.Data;
    using GamersHub.Web.ViewModels.Administration.Dashboard;
    using Microsoft.AspNetCore.Mvc;

    public class DashboardController : AdministrationController
    {
        private readonly IForumsService forumsService;
        private readonly ICategoriesService categoriesService;

        public DashboardController(IForumsService forumsService, ICategoriesService categoriesService)
        {
            this.forumsService = forumsService;
            this.categoriesService = categoriesService;
        }

        public IActionResult Index()
        {
            var forums = this.forumsService.GetAll<ForumDashboardViewModel>()
                .OrderByDescending(x=>x.PostsCount)
                .Take(5);
            var categories = this.categoriesService.GetAll<CategoryDashboardViewModel>()
                .OrderByDescending(x=>x.PostsCount)
                .Take(5);

            var viewModel = new IndexViewModel
            {
                Forums = forums,
                Categories = categories,
            };

            return this.View(viewModel);
        }
    }
}