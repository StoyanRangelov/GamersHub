using System;
using System.ComponentModel.DataAnnotations;
using GamersHub.Common;
using GamersHub.Data.Models;
using GamersHub.Services.Mapping;

namespace GamersHub.Web.ViewModels.Posts
{
   public class CreatePostInputModel
    {
        [Required]
        public string ForumName { get; set; }

        [Required]
        public string CategoryName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = GlobalConstants.StringLengthErrorMessage, MinimumLength = 4)]
        public string Name { get; set; }

        [Required]
        [StringLength(20000, ErrorMessage = GlobalConstants.StringLengthErrorMessage, MinimumLength = 10)]
        public string Content { get; set; }
    }
}
