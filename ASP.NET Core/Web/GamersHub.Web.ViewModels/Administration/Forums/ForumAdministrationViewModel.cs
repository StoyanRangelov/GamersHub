using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using GamersHub.Common;
using GamersHub.Data.Models;
using GamersHub.Services.Mapping;
using GamersHub.Web.ViewModels.Forums;

namespace GamersHub.Web.ViewModels.Administration.Forums
{
    public class ForumAdministrationViewModel : IMapFrom<Forum>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Url => UrlParser.ParseToUrl(this.Name);

        public int PostsCount { get; set; }

        public int ForumCategoriesCount { get; set; }
    }
}