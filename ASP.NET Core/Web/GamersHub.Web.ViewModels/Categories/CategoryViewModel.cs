using System;
using System.Collections.Generic;
using System.Text;
using GamersHub.Data.Models;
using GamersHub.Services.Mapping;

namespace GamersHub.Web.ViewModels.Categories
{
   public class CategoryViewModel : IMapFrom<Category>
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public int CategoryForumsCount { get; set; }

        public int PostsCount { get; set; }
    }
}
