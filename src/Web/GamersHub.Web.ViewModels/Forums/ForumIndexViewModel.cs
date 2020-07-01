namespace GamersHub.Web.ViewModels.Forums
{
    using System.Collections.Generic;

    public class ForumIndexViewModel
    {
        public PaginationViewModel Pagination { get; set; }

        public IEnumerable<ForumViewModel> Forums { get; set; }
    }
}
