namespace GamersHub.Web.ViewModels.Administration.Categories
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using GamersHub.Common;
    using GamersHub.Data.Models;
    using GamersHub.Services.Mapping;

    public class CategoryEditInputModel : IMapFrom<Category>
    {
        [Required]
        [StringLength(60, ErrorMessage = GlobalConstants.StringLengthErrorMessage, MinimumLength = 3)]
        public string Name { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = GlobalConstants.StringLengthErrorMessage, MinimumLength = 5)]
        public string Description { get; set; }

        public IEnumerable<ForumInCategoryViewModel> CategoryForums { get; set; }
    }
}
