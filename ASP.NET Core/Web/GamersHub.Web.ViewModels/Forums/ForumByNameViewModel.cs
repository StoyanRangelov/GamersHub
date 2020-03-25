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
   public class ForumByNameViewModel : IMapFrom<Forum>, IHaveCustomMappings
    {
        public string Name { get; set; }

        public string Url => UrlParser.ParseToUrl(this.Name);

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

        public IEnumerable<PostInForumViewModel> Posts { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Forum, ForumByNameViewModel>()
                .ForMember(x => x.CategoryNames, y => y
                    .MapFrom(x => x.ForumCategories.Select(fc => fc.Category.Name)))
                .ForMember(x => x.Posts, y=>y.
                    MapFrom(x => x.Posts.OrderByDescending(p => p.CreatedOn)));
        }
    }
}
