﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GamersHub.Common;
using GamersHub.Data.Models;
using GamersHub.Services.Mapping;

namespace GamersHub.Web.ViewModels.Posts
{
   public class CreatePostInputModel
    {

        [Display(Name = "Forum")]
        public int ForumId { get; set; }

        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = GlobalConstants.StringLengthErrorMessage, MinimumLength = 4)]
        public string Name { get; set; }

        [Required]
        public string Content { get; set; }

        public IEnumerable<CategoryDropDownViewModel> Categories { get; set; }

        public IEnumerable<ForumDropDownViewModel> Forums { get; set; }
    }
}
