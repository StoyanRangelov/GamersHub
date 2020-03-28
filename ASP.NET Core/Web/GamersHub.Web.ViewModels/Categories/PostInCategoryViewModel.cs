﻿using System;
using System.Collections.Generic;
using GamersHub.Common;
using GamersHub.Data.Models;
using GamersHub.Services.Mapping;

namespace GamersHub.Web.ViewModels.Categories
{
    public class PostInCategoryViewModel : IMapFrom<Category>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime CreatedOn { get; set; }

        public string Url => UrlParser.ParseToUrl(this.Name);

        public string ShortName =>
            this.Name?.Length > 40
                ? this.Name?.Substring(0, 40) + "..."
                : this.Name;

        public string CategoryName { get; set; }

        public string UserUsername { get; set; }

        public int RepliesCount { get; set; }
    }
}