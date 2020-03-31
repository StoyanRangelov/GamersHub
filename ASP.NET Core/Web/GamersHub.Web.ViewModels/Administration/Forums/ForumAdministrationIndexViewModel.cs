using System.Collections.Generic;

namespace GamersHub.Web.ViewModels.Administration.Forums
{
    public class ForumAdministrationIndexViewModel
    {
        public IEnumerable<ForumAdministrationViewModel> Forums { get; set; }
    }
}