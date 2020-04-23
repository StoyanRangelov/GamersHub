namespace GamersHub.Web.ViewModels.Administration.Forums
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using GamersHub.Common;
    using GamersHub.Data.Models;
    using GamersHub.Services.Mapping;

    public class ForumEditInputModel : IMapFrom<Forum>
    {
        [Required]
        [Display(Name ="Name")]
        [StringLength(60, ErrorMessage = GlobalConstants.StringLengthErrorMessage, MinimumLength = 3)]
        public string Name { get; set; }

        public IEnumerable<CategoryInForumViewModel> ForumCategories { get; set; }
    }
}
