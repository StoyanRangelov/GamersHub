using System;
using GamersHub.Data.Models;
using GamersHub.Services.Mapping;

namespace GamersHub.Web.ViewModels.Administration.Users
{
    public class UserBannedViewModel : IMapFrom<ApplicationUser>
    {
        public string Id { get; set; }

        public string Username { get; set; }

        public DateTimeOffset LockoutEnd { get; set; }
    }
}