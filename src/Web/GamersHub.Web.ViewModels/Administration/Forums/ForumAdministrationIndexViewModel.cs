namespace GamersHub.Web.ViewModels.Administration.Forums
{
    using System.Collections.Generic;

    public class ForumAdministrationIndexViewModel
    {
        public PaginationViewModel Pagination { get; set; }

        public IEnumerable<ForumAdministrationViewModel> Forums { get; set; }
    }
}
