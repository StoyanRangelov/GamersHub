﻿using System.Linq;
using AutoMapper;
using GamersHub.Data.Models;
using GamersHub.Services.Mapping;

namespace GamersHub.Web.ViewModels.Administration.Forums
{
    public class CategoryInForumViewModel : IMapFrom<ForumCategory>
    {
        public string CategoryName { get; set; }
    }
}