using System;
using System.Text;
using System.Text.RegularExpressions;
using AutoMapper;
using GamersHub.Data.Models;
using GamersHub.Services.Mapping;

namespace GamersHub.Web.ViewModels.Forums
{
    public class PostInForumViewModel : IMapFrom<Post>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Url
        {
            get
            {
                var matches = Regex.Matches(this.Name, "[^!*'();:@&=+$,/?#[\\]]+");

                var result = new StringBuilder();

                foreach (Match match in matches)
                {
                    result.Append(match.Value);
                }

                result.Replace(' ', '-');

                return result.ToString().ToLower();
            }
        }


        public string ShortName =>
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
                    .MapFrom(y => y.CreatedOn.ToString("MM/dd/yyyy hh:mm tt")));
        }
    }
}
