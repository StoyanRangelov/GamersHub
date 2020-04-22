using System;
using System.ComponentModel.DataAnnotations;
using GamersHub.Data.Models;
using GamersHub.Services.Mapping;
using Ganss.XSS;

namespace GamersHub.Web.ViewModels.Replies
{
    public class ReplyDeleteViewModel : IMapFrom<Reply>
    {
        public int Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public string Content { get; set; }

        public string SanitizedContent => new HtmlSanitizer().Sanitize(this.Content);

        public string PostUrl { get; set; }

        public string UserId { get; set; }

        public string UserUsername { get; set; }

        public string UserImgUrl { get; set; }

        public int UserPostsCount { get; set; }

        public int UserRepliesCount { get; set; }

        public int Publications => this.UserPostsCount + this.UserRepliesCount;

        public ReplyPostViewModel Post { get; set; }
    }
}