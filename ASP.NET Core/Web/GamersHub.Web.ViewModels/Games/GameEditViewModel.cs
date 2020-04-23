namespace GamersHub.Web.ViewModels.Games
{
    using System.ComponentModel.DataAnnotations;

    using GamersHub.Common;
    using GamersHub.Data.Models;
    using GamersHub.Services.Mapping;
    using Microsoft.AspNetCore.Http;

    public class GameEditViewModel : IMapFrom<Game>
    {
        public int Id { get; set; }

        [Required]
        [StringLength(60, ErrorMessage = GlobalConstants.StringLengthErrorMessage, MinimumLength = 3)]
        public string Title { get; set; }

        public string Url => UrlParser.ParseToUrl(this.Title);

        [Display(Name = "Sub Title")]
        [StringLength(60, ErrorMessage = GlobalConstants.StringLengthErrorMessage, MinimumLength = 3)]
        public string SubTitle { get; set; }

        [Required]
        public string Description { get; set; }

        public string ImgUrl { get; set; }

        public IFormFile Image { get; set; }
    }
}
