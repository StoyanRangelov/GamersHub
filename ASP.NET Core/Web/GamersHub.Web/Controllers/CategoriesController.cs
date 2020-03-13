using GamersHub.Services.Data;
using GamersHub.Web.ViewModels.Categories;
using Microsoft.AspNetCore.Mvc;

namespace GamersHub.Web.Controllers
{
    public class CategoriesController : BaseController
    {
        private readonly ICategoriesService categoriesService;

        public CategoriesController(ICategoriesService categoriesService)
        {
            this.categoriesService = categoriesService;
        }

        public IActionResult Index()
        {
            var viewModel = new CategoryIndexViewModel
            {
                Categories = this.categoriesService.GetAll<CategoryViewModel>(),
            };

            return this.View(viewModel);
        }

        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        public IActionResult Create(CategoryCreateInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            this.categoriesService.Create(inputModel.Name, inputModel.Description);

            return this.Redirect("/Categories");
        }
    }
}
