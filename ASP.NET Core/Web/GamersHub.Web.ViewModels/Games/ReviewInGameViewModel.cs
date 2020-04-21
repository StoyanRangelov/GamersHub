using System;
using GamersHub.Data.Models;
using GamersHub.Services.Mapping;
using Ganss.XSS;

namespace GamersHub.Web.ViewModels.Games
{
    public class ReviewInGameViewModel : IMapFrom<Review>
    {
        public int Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public string Content { get; set; }

        public string SanitizedContent => new HtmlSanitizer().Sanitize(this.Content);

        public string UserUsername { get; set; }

        public string UserImgUrl { get; set; }

        public string UserReviewsCount { get; set; }

        public bool IsPositive { get; set; }
    }
}