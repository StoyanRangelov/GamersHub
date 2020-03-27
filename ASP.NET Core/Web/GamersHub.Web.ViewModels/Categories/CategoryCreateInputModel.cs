using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using GamersHub.Common;

namespace GamersHub.Web.ViewModels.Categories
{
   public class CategoryCreateInputModel
    {
        [Required]
        [StringLength(60, ErrorMessage = GlobalConstants.StringLengthErrorMessage, MinimumLength = 4)]
        public string Name { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = GlobalConstants.StringLengthErrorMessage, MinimumLength = 5)]
        public string Description { get; set; }
    }
}
