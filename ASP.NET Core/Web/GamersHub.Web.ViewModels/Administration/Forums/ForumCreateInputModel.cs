namespace GamersHub.Web.ViewModels.Administration.Forums
{
    using System.ComponentModel.DataAnnotations;

    using GamersHub.Common;

    public class ForumCreateInputModel
    {
        [Required]
        [StringLength(60, ErrorMessage = GlobalConstants.StringLengthErrorMessage, MinimumLength = 3)]
        public string Name { get; set; }
    }
}
