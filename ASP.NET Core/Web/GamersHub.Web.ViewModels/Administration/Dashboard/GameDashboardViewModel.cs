using GamersHub.Data.Models;
using GamersHub.Services.Mapping;

namespace GamersHub.Web.ViewModels.Administration.Dashboard
{
    public class GameDashboardViewModel : IMapFrom<Game>
    {
        public string Title { get; set; }
        
        public int ReviewsCount { get; set; }
    }
}