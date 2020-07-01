namespace GamersHub.Web.ViewModels.Administration.Categories
{
    using System.Collections.Generic;

    public class CategoryIndexViewModel
    {
        public PaginationViewModel Pagination { get; set; }

        public IEnumerable<CategoryViewModel> Categories { get; set; }
    }
}
