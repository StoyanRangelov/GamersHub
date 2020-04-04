using System.Collections.Generic;

namespace GamersHub.Web.ViewModels.Administration.Dashboard
{
    public class IndexViewModel
    {
        public IEnumerable<ForumDashboardViewModel> Forums { get; set; }

        public IEnumerable<CategoryDashboardViewModel> Categories { get; set; }

        public IEnumerable<PostDashboardViewModel> Posts { get; set; }
    }
}
