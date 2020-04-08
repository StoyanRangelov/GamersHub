using System;
using GamersHub.Data.Models;
using GamersHub.Services.Mapping;

namespace GamersHub.Web.ViewModels.Administration.Moderators
{
    public class ModeratorDemoteViewModel : IMapFrom<ApplicationUser>
    {
        public string Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public string Username { get; set; }

        public int PostsCount { get; set; }

        public int RepliesCount { get; set; }
    }
}