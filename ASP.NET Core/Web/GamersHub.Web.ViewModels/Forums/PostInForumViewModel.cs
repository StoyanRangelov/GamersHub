using System;
using AutoMapper;
using GamersHub.Data.Models;
using GamersHub.Services.Mapping;

namespace GamersHub.Web.ViewModels.Forums
{
    public class PostInForumViewModel : IMapFrom<Post>, IHaveCustomMappings
    {
        public string Name { get; set; }

        public string ShortTopic =>
            this.Name?.Length > 40
                ? this.Name?.Substring(0, 40) + "..."
                : this.Name;

        public string CategoryName { get; set; }

        public string UserUsername { get; set; }

        public int RepliesCount { get; set; }

        public string CreatedOn { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Post, PostInForumViewModel>()
                .ForMember(x => x.CreatedOn, y => y
                    .MapFrom(y => y.CreatedOn.ToString("yyyy/dd/MM H:mm:ss")));
        }
    }
}
