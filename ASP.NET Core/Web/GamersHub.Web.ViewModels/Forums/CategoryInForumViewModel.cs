﻿using GamersHub.Common;
using GamersHub.Data.Models;
using GamersHub.Services.Mapping;

namespace GamersHub.Web.ViewModels.Forums
{
    public class CategoryInForumViewModel : IMapFrom<ForumCategory>
    {
        public int CategoryId { get; set; }

        public string CategoryName { get; set; }

        public string Url => UrlParser.ParseToUrl(this.CategoryName);

        public string CategoryDescription { get; set; }
    }
}