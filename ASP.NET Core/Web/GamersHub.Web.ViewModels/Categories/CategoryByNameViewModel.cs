using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using GamersHub.Common;
using GamersHub.Data.Models;
using GamersHub.Services.Mapping;
using GamersHub.Web.ViewModels.Forums;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace GamersHub.Web.ViewModels.Categories
{
    public class CategoryByNameViewModel
    {
        public string ForumName { get; set; }

        public string CategoryName { get; set; }

        public IEnumerable<PostInCategoryViewModel> CategoryPosts { get; set; }
    }
}
