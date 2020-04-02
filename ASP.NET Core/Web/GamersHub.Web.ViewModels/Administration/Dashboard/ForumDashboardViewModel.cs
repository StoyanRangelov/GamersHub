using GamersHub.Data.Models;
using GamersHub.Services.Mapping;

namespace GamersHub.Web.ViewModels.Administration.Dashboard
{
    public class ForumDashboardViewModel : IMapFrom<Forum>
    {
        public string Name { get; set; }

        public int PostsCount { get; set; }
    }
}