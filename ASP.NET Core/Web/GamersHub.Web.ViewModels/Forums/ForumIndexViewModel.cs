using System.Collections.Generic;

namespace GamersHub.Web.ViewModels.Forums
{
   public class ForumIndexViewModel
    {
        public PaginationViewModel Pagination { get; set; }

        public IEnumerable<ForumViewModel> Forums { get; set; }
    }
}
