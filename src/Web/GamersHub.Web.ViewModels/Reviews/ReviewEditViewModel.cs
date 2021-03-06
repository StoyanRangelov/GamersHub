﻿namespace GamersHub.Web.ViewModels.Reviews
{
    using System.ComponentModel.DataAnnotations;

    using GamersHub.Common;
    using GamersHub.Data.Models;
    using GamersHub.Services.Mapping;

    public class ReviewEditViewModel : IMapFrom<Review>
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public string GameTitle { get; set; }

        public string GameUrl => UrlParser.ParseToUrl(this.GameTitle);

        public string GameSubTitle { get; set; }

        [Required]
        public string Content { get; set; }

        public bool IsPositive { get; set; }
    }
}
