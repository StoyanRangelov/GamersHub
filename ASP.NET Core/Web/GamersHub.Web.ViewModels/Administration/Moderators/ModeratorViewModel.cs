namespace GamersHub.Web.ViewModels.Administration.Moderators
{
    using System;

    using GamersHub.Data.Models;
    using GamersHub.Services.Mapping;

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

        public int ReviewsCount { get; set; }

        public int PartiesCount { get; set; }
    }
}
