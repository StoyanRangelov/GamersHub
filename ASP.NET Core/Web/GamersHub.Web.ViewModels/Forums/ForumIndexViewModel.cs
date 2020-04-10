using System.Collections.Generic;

namespace GamersHub.Web.ViewModels.Forums
{
   public class ForumIndexViewModel
    {
        public int PagesCount { get; set; }

        public int CurrentPage { get; set; }

        public IEnumerable<ForumViewModel> Forums { get; set; }
    }
}
