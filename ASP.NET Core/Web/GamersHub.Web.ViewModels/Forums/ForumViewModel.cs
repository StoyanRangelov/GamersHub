using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using GamersHub.Data.Models;
using GamersHub.Services.Mapping;

namespace GamersHub.Web.ViewModels.Forums
{
    public class ForumViewModel : IMapFrom<Forum>, IHaveCustomMappings
    {
        public string Name { get; set; }

        public int PostsCount { get; set; }

        public string[] CategoryNames { get; set; }

        public string[] CategoryDescriptions { get; set; }

        public string Url => $"{this.Name.Replace(' ', '-')}";

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Forum, ForumViewModel>()
                .ForMember(x => x.CategoryNames, y => y
                    .MapFrom(x => x.ForumCategories.Select(fc => fc.Category.Name)))
                .ForMember(x => x.CategoryDescriptions, y => y
                    .MapFrom(x => x.ForumCategories.Select(fc => fc.Category.Description)));
        }
    }
}
