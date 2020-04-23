namespace GamersHub.Web.ViewModels.Administration.Dashboard
{
    using GamersHub.Data.Models;
    using GamersHub.Services.Mapping;

    public class ForumDashboardViewModel : IMapFrom<Forum>
    {
        public string Name { get; set; }

        public int PostsCount { get; set; }
    }
}
