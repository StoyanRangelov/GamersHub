﻿namespace GamersHub.Web.ViewModels.Administration.Users
{
    using System;

    using GamersHub.Data.Models;
    using GamersHub.Services.Mapping;

    public class UserBanViewModel : IMapFrom<ApplicationUser>
    {
        public string Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public string Username { get; set; }

        public string ImgUrl { get; set; }

        public int PostsCount { get; set; }

        public int RepliesCount { get; set; }

        public int ReviewsCount { get; set; }

        public int PartiesCount { get; set; }
    }
}
