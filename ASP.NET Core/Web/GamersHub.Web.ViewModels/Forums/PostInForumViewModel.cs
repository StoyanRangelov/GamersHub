using System;
using AutoMapper;
using GamersHub.Data.Models;
using GamersHub.Services.Mapping;

namespace GamersHub.Web.ViewModels.Forums
{
    public class PostInForumViewModel : IMapFrom<Post>, IHaveCustomMappings
    {
        public string Topic { get; set; }

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
