using System;
using System.Collections;
using System.Collections.Generic;
using AutoMapper;
using GamersHub.Data.Models;
using GamersHub.Services.Mapping;

namespace GamersHub.Web.ViewModels.Posts
{
    public class PostDetailsViewModel : IMapFrom<Post>
    {
        public string Name { get; set; }

        public string Content { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public string UserUsername { get; set; }

        public int UserPostsCount { get; set; }

        public int UserRepliesCount { get; set; }

        public int Publications => this.UserPostsCount + this.UserRepliesCount;

        public string ForumName { get; set; }

        public string CategoryName { get; set; }

        public IEnumerable<ReplyInPostViewModel> Replies { get; set; }
    }
}