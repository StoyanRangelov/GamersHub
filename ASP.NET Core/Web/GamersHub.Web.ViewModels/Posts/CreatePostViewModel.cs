using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using GamersHub.Data.Models;
using GamersHub.Services.Mapping;

namespace GamersHub.Web.ViewModels.Posts
{
   public class CreatePostViewModel : IMapFrom<Forum>, IHaveCustomMappings
    {
        public string Name { get; set; }

        public string[] CategoryNames { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Forum, CreatePostViewModel>()
                .ForMember(x => x.CategoryNames, y => y
                    .MapFrom(x => x.ForumCategories.Select(fc => fc.Category.Name)));
        }
    }
}
