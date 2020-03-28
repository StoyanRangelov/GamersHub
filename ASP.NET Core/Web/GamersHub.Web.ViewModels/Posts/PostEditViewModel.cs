using System.ComponentModel.DataAnnotations;
using GamersHub.Common;
using GamersHub.Data.Models;
using GamersHub.Services.Mapping;

namespace GamersHub.Web.ViewModels.Posts
{
    public class PostEditViewModel : IMapFrom<Post>
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = GlobalConstants.StringLengthErrorMessage, MinimumLength = 3)]
        public string Name { get; set; }

        public string Url => UrlParser.ParseToUrl(this.Name);

        [Required]

        public string Content { get; set; }
    }
}