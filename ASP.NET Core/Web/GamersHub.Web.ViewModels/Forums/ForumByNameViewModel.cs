﻿using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using AutoMapper;
using GamersHub.Common;
using GamersHub.Data.Models;
using GamersHub.Services.Mapping;

namespace GamersHub.Web.ViewModels.Forums
{
    public class ForumByNameViewModel : IMapFrom<Forum>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Url => UrlParser.ParseToUrl(this.Name);

        public int PagesCount { get; set; }

        public int CurrentPage { get; set; }

        public IEnumerable<CategoryInForumByNameViewModel> ForumCategories { get; set; }

        public IEnumerable<PostInForumViewModel> ForumPosts { get; set; }
    }
}