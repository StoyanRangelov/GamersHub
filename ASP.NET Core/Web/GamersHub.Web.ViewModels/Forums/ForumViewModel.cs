using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using AutoMapper;
using GamersHub.Common;
using GamersHub.Data.Models;
using GamersHub.Services.Mapping;

namespace GamersHub.Web.ViewModels.Forums
{
    public class ForumViewModel : IMapFrom<Forum>, IHaveCustomMappings
    {
        public string Name { get; set; }

        public string Url => UrlParser.ParseToUrl(this.Name);

        public int PostsCount { get; set; }

        public string[] CategoryNames { get; set; }

        public string[] CategoryUrls
        {
            get
            {
                var categoryUrls = new string[this.CategoryNames.Length];

                for (int i = 0; i < this.CategoryNames.Length; i++)
                {
                    var categoryUrl = UrlParser.ParseToUrl(this.CategoryNames[i]);

                    categoryUrls[i] = categoryUrl;
                }

                return categoryUrls;
            }
        }

        public string[] CategoryDescriptions { get; set; }

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