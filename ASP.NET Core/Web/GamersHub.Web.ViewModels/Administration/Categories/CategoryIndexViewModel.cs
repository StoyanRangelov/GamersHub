﻿using System.Collections.Generic;
using GamersHub.Web.ViewModels.Categories;

namespace GamersHub.Web.ViewModels.Administration.Categories
{
   public class CategoryIndexViewModel
    {
        public int PagesCount { get; set; }
        
        public int CurrentPage { get; set; }
        public IEnumerable<CategoryViewModel> Categories { get; set; }
    }
}
