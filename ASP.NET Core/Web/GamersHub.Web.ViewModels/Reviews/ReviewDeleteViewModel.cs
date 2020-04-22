using System;
using GamersHub.Common;
using GamersHub.Data.Models;
using GamersHub.Services.Mapping;
using Ganss.XSS;

namespace GamersHub.Web.ViewModels.Reviews
{
    public class ReviewDeleteViewModel : IMapFrom<Review>
    {
        public int Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public string GameTitle { get; set; }

        public string GameUrl => UrlParser.ParseToUrl(this.GameTitle);

        public string GameSubTitle { get; set; }

        public string Content { get; set; }

        public string SanitizedContent => new HtmlSanitizer().Sanitize(this.Content);

        public string UserId { get; set; }

        public string UserUsername { get; set; }

        public string UserImgUrl { get; set; }

        public string UserReviewsCount { get; set; }

        public bool IsPositive { get; set; }
    }
}