using System.Collections;
using System.Collections.Generic;
using AutoMapper;
using GamersHub.Data.Models;
using GamersHub.Services.Mapping;

namespace GamersHub.Web.ViewModels.Posts
{
    public class PostDetailsViewModel : IMapFrom<Post>, IHaveCustomMappings
    {
        public string Name { get; set; }

        public string Content { get; set; }

        public string CreatedOn { get; set; }

        public string ModifiedOn { get; set; }

        public string UserUsername { get; set; }

        public int UserPostsCount { get; set; }

        public int UserRepliesCount { get; set; }

        public int Publications => this.UserPostsCount + this.UserRepliesCount;

        public string ForumName { get; set; }

        public string CategoryName { get; set; }

        public IEnumerable<ReplyInPostViewModel> Replies { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Post, PostDetailsViewModel>()
                .ForMember(x => x.CreatedOn, y => y
                    .MapFrom(y => y.CreatedOn.ToString("yyyy/dd/MM H:mm:ss")))
                .ForMember(x => x.ModifiedOn, y => y
                    .MapFrom(y => y.ModifiedOn != null ? y.ModifiedOn.Value.ToString("yyyy/dd/MM H:mm:ss") : null));
        }
    }
}