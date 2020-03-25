using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using AutoMapper;
using GamersHub.Data.Models;
using GamersHub.Services.Mapping;

namespace GamersHub.Web.ViewModels.Forums
{
   public class ForumByNameViewModel : IMapFrom<Forum>, IHaveCustomMappings
    {
        public string Name { get; set; }

        public string Url
        {
            get
            {
                var matches = Regex.Matches(this.Name, "[^!*'();:@&=+$,/?#[\\]]+");

                var result = new StringBuilder();

                foreach (Match match in matches)
                {
                    if (match.Value.EndsWith(' '))
                    {
                        match.Value.Remove(match.Value.Length - 1);
                    }

                    result.Append(match.Value);
                }

                result.Replace(' ', '-');

                return result.ToString();
            }
        }

        public string[] CategoryNames { get; set; }

        public string[] CategoryNamesUrls
        {
            get
            {
                var categoryUrls = new string[this.CategoryNames.Length];

                for (int i = 0; i < this.CategoryNames.Length; i++)
                {
                    var matches = Regex.Matches(this.CategoryNames[i], "[^!*'();:@&=+$,/?#[\\]]+");

                    var result = new StringBuilder();

                    foreach (Match match in matches)
                    {
                        if (match.Value.EndsWith(' '))
                        {
                            match.Value.Remove(match.Value.Length - 1);
                        }
                        result.Append(match.Value);
                    }

                    result.Replace(' ', '-');

                    categoryUrls[i] = result.ToString();
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
