namespace GamersHub.Web.ViewModels.Administration.Categories
{
    using System.ComponentModel.DataAnnotations;

    using GamersHub.Common;

    public class CategoryCreateInputModel
    {
        [Required]
        [StringLength(60, ErrorMessage = GlobalConstants.StringLengthErrorMessage, MinimumLength = 3)]
        public string Name { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = GlobalConstants.StringLengthErrorMessage, MinimumLength = 5)]
        public string Description { get; set; }
    }
}
