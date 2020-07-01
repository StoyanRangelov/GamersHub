namespace GamersHub.Web.ViewModels.Parties.InputModels
{
    using System.ComponentModel.DataAnnotations;

    using GamersHub.Common;
    using GamersHub.Data.Models;

    public class PartyCreateInputModel
    {
        [Required]
        [StringLength(100, ErrorMessage = GlobalConstants.StringLengthErrorMessage, MinimumLength = 3)]
        public string Game { get; set; }

        [Required]
        [StringLength(400, ErrorMessage = GlobalConstants.StringLengthErrorMessage, MinimumLength = 3)]
        public string Description { get; set; }

        [Required]
        [Range(1, 40)]
        public int Size { get; set; }

        public ActivityType Activity { get; set; }
    }
}
