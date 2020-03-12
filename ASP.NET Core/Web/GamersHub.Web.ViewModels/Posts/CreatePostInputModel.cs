using System.ComponentModel.DataAnnotations;
using GamersHub.Data.Models;
using GamersHub.Services.Mapping;

namespace GamersHub.Web.ViewModels.Posts
{
   public class CreatePostInputModel
    {
        public string ForumName { get; set; }

        public string CategoryName { get; set; }

        [Required]
        public string Topic { get; set; }

        [Required]
        public string Content { get; set; }
    }
}
