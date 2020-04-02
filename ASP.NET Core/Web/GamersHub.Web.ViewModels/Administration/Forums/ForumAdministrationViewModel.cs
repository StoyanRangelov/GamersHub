using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using GamersHub.Data.Models;
using GamersHub.Services.Mapping;
using GamersHub.Web.ViewModels.Forums;

namespace GamersHub.Web.ViewModels.Administration.Forums
{
    public class ForumAdministrationViewModel : IMapFrom<Forum>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int PostsCount { get; set; }

        public int ForumCategoriesCount { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Forum, ForumAdministrationViewModel>()
                .ForMember(x => x.ForumCategoriesCount, y => y
                    .MapFrom(x => x.ForumCategories.Count(x => x.Category.IsDeleted == false)));
        }
    }
}