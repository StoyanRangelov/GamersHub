using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using GamersHub.Common;
using GamersHub.Data.Models;
using GamersHub.Services.Mapping;
using Ganss.XSS;

namespace GamersHub.Web.ViewModels.Posts
{
    public class PostByNameViewModel : IMapFrom<Post>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Url => UrlParser.ParseToUrl(this.Name).ToLower();

        public string Content { get; set; }

        public string SanitizedContent => new HtmlSanitizer().Sanitize(this.Content);

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public string UserUsername { get; set; }

        public int UserPostsCount { get; set; }

        public int UserRepliesCount { get; set; }

        public int Publications => this.UserPostsCount + this.UserRepliesCount;

        public string ForumName { get; set; }

        public string ForumUrl => UrlParser.ParseToUrl(this.ForumName);

        public string CategoryName { get; set; }

        public IEnumerable<ReplyInPostViewModel> PostReplies { get; set; }

        public PaginationViewModel Pagination { get; set; }
    }
}