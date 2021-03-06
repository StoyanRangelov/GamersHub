﻿namespace GamersHub.Web.ViewModels.Administration.Dashboard
{
    using GamersHub.Data.Models;
    using GamersHub.Services.Mapping;

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
