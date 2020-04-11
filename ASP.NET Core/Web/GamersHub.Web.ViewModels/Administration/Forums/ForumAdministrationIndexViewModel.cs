using System.Collections.Generic;

namespace GamersHub.Web.ViewModels.Administration.Forums
{
    public class ForumAdministrationIndexViewModel
    {
        public PaginationViewModel Pagination { get; set; }

        public IEnumerable<ForumAdministrationViewModel> Forums { get; set; }
    }
}