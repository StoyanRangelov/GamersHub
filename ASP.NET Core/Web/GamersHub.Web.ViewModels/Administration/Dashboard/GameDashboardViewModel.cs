namespace GamersHub.Web.ViewModels.Administration.Dashboard
{
    using GamersHub.Data.Models;
    using GamersHub.Services.Mapping;

    public class GameDashboardViewModel : IMapFrom<Game>
    {
        public string Title { get; set; }

        public int ReviewsCount { get; set; }
    }
}
