namespace GamersHub.Web.ViewModels.Administration.Users
{
    using System;

    using GamersHub.Data.Models;
    using GamersHub.Services.Mapping;

    public class UserBannedViewModel : IMapFrom<ApplicationUser>
    {
        public string Id { get; set; }

        public string Username { get; set; }

        public DateTimeOffset LockoutEnd { get; set; }
    }
}
