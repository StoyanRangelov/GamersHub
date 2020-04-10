using System.Collections.Generic;

namespace GamersHub.Web.ViewModels.Administration.Forums
{
    public class ForumAdministrationIndexViewModel
    {
        public int PagesCount { get; set; }

        public int CurrentPage { get; set; }
        public IEnumerable<ForumAdministrationViewModel> Forums { get; set; }
    }
}