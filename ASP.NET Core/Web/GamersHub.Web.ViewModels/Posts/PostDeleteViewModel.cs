using System;
using GamersHub.Common;
using GamersHub.Data.Models;
using GamersHub.Services.Mapping;
using Ganss.XSS;

namespace GamersHub.Web.ViewModels.Posts
{
    public class PostDeleteViewModel : IMapFrom<Post>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Content { get; set; }

        public string SanitizedContent => new HtmlSanitizer().Sanitize(this.Content);

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public string UserUsername { get; set; }

        public string UserImgUrl { get; set; }

        public int UserPostsCount { get; set; }

        public int UserRepliesCount { get; set; }

        public int Publications => this.UserPostsCount + this.UserRepliesCount;

        public string ForumName { get; set; }
    }
}