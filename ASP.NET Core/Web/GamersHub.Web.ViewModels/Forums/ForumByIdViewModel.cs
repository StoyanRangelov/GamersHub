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
   public class ForumByIdViewModel : IMapFrom<Forum>
    {
        public string Name { get; set; }

        public string Url => UrlParser.ParseToUrl(this.Name);

        public IEnumerable<CategoryInForumByIdViewModel> ForumCategories { get; set; }

        public IEnumerable<PostInForumViewModel> Posts { get; set; }
    }
}
