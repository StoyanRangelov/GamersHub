using GamersHub.Data.Models;
using GamersHub.Services.Mapping;

namespace GamersHub.Web.ViewModels.Administration.Dashboard
{
    public class GameUserDashboardViewModel : IMapFrom<ApplicationUser>
    {
        public string Username { get; set; }

        public string ShortUsername =>
            this.Username?.Length > 30
                ? this.Username?.Substring(0, 30) + "..."
                : this.Username;

        public int ReviewsCount { get; set; }
    }
}