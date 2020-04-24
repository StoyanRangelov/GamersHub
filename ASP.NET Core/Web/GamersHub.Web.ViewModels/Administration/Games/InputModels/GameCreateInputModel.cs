namespace GamersHub.Web.ViewModels.Administration.Games.InputModels
{
    using System.ComponentModel.DataAnnotations;

    using GamersHub.Common;
    using Microsoft.AspNetCore.Http;

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
