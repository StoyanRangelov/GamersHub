﻿using System.ComponentModel.DataAnnotations;
using GamersHub.Common;

namespace GamersHub.Web.ViewModels.Forums
{
    public class ForumCreateInputModel
    {
        [Required]
        [StringLength(60, ErrorMessage = GlobalConstants.StringLengthErrorMessage, MinimumLength = 4)]
        public string Name { get; set; }
    }
}
