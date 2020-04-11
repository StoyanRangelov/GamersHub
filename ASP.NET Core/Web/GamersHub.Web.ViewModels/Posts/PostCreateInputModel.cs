using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GamersHub.Common;
using GamersHub.Data.Models;
using GamersHub.Services.Mapping;

namespace GamersHub.Web.ViewModels.Posts
{
   public class PostCreateInputModel
    {

        [Display(Name = "Forum")]
        [Range(1, int.MaxValue)]
        public int ForumId { get; set; }

        [Display(Name = "Category")]
        [Range(1, int.MaxValue)]
        public int CategoryId { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = GlobalConstants.StringLengthErrorMessage, MinimumLength = 3)]
        public string Name { get; set; }

        public string Url => UrlParser.ParseToUrl(this.Name).ToLower();

        [Required]
        public string Content { get; set; }

        public IEnumerable<CategoryDropDownViewModel> Categories { get; set; }

        public IEnumerable<ForumDropDownViewModel> Forums { get; set; }
    }
}
