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
    public class ForumViewModel : IMapFrom<Forum>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Url => UrlParser.ParseToUrl(this.Name);

        public int PostsCount { get; set; }


        public IEnumerable<CategoryInForumViewModel> ForumCategories { get; set; }
    }
}