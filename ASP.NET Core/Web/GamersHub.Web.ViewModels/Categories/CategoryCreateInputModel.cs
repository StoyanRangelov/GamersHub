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
        [StringLength(30, ErrorMessage = GlobalConstants.StringLengthErrorMessage, MinimumLength = 3)]
        public string Name { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = GlobalConstants.StringLengthErrorMessage, MinimumLength = 5)]
        public string Description { get; set; }
    }
}
