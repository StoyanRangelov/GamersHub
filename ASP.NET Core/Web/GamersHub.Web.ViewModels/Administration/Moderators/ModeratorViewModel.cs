using System;
using GamersHub.Data.Models;
using GamersHub.Services.Mapping;

namespace GamersHub.Web.ViewModels.Administration.Moderators
{
    public class ModeratorViewModel : IMapFrom<ApplicationUser>
    {
        public string Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public string Username { get; set; }

        public string ShortUsername =>
            this.Username?.Length > 20
                ? this.Username?.Substring(0, 20) + "..."
                : this.Username;

        public int PostsCount { get; set; }

        public int RepliesCount { get; set; }
    }
}