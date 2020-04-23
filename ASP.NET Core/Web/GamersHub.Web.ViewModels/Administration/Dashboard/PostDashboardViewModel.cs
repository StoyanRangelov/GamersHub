namespace GamersHub.Web.ViewModels.Administration.Dashboard
{
    using GamersHub.Data.Models;
    using GamersHub.Services.Mapping;

    public class PostDashboardViewModel : IMapFrom<Post>
    {
        public string Name { get; set; }

        public string ShortName =>
            this.Name?.Length > 30
                ? this.Name?.Substring(0, 30) + "..."
                : this.Name;

        public int RepliesCount { get; set; }
    }
}
