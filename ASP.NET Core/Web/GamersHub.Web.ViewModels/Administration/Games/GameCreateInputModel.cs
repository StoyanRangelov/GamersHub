using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Net;
using AngleSharp.Io.Dom;
using GamersHub.Common;
using Microsoft.AspNetCore.Http;

namespace GamersHub.Web.ViewModels.Administration.Games
{
    public class GameCreateInputModel
    {
        [Required]
        [StringLength(60, ErrorMessage = GlobalConstants.StringLengthErrorMessage, MinimumLength = 3)]
        public string Title { get; set; }

        [Display(Name = "Sub Title")]
        [StringLength(60, ErrorMessage = GlobalConstants.StringLengthErrorMessage, MinimumLength = 3)]
        public string SubTitle { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public IFormFile Image { get; set; }
    }
}
