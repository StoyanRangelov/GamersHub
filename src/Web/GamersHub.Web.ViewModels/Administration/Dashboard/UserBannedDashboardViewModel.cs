namespace GamersHub.Web.ViewModels.Administration.Dashboard
{
    using System;

    using GamersHub.Data.Models;
    using GamersHub.Services.Mapping;

    public class UserBannedDashboardViewModel : IMapFrom<ApplicationUser>
    {
        public string Username { get; set; }

        public string ShortUsername =>
            this.Username?.Length > 20
                ? this.Username?.Substring(0, 20) + "..."
                : this.Username;

        public DateTimeOffset LockoutEnd { get; set; }
    }
}
