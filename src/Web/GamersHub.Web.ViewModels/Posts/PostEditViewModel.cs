namespace GamersHub.Web.ViewModels.Posts
{
    using System.ComponentModel.DataAnnotations;

    using GamersHub.Common;
    using GamersHub.Data.Models;
    using GamersHub.Services.Mapping;

    public class PostEditViewModel : IMapFrom<Post>
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = GlobalConstants.StringLengthErrorMessage, MinimumLength = 3)]
        public string Name { get; set; }

        public string Url => UrlParser.ParseToUrl(this.Name).ToLower();

        [Required]

        public string Content { get; set; }
    }
}
