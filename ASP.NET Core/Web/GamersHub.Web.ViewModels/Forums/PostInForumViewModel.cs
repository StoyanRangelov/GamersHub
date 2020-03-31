using System;
using System.Text;
using System.Text.RegularExpressions;
using AutoMapper;
using GamersHub.Common;
using GamersHub.Data.Models;
using GamersHub.Services.Mapping;

namespace GamersHub.Web.ViewModels.Forums
{
    public class PostInForumViewModel : IMapFrom<Post>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime CreatedOn { get; set; }

        public string Url => UrlParser.ParseToUrl(this.Name);

        public string ShortName =>
            this.Name?.Length > 60
                ? this.Name?.Substring(0, 60) + "..."
                : this.Name;

        public string CategoryName { get; set; }

        public string UserUsername { get; set; }

        public int RepliesCount { get; set; }
    }
}
