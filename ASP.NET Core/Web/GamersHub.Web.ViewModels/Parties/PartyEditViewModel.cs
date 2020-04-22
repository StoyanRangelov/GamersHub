using System.ComponentModel.DataAnnotations;
using GamersHub.Common;
using GamersHub.Data.Models;
using GamersHub.Services.Mapping;

namespace GamersHub.Web.ViewModels.Parties
{
    public class PartyEditViewModel : IMapFrom<Party>
    {
        public int Id { get; set; }

        public string CreatorId { get; set; }

        public string CreatorUsername { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = GlobalConstants.StringLengthErrorMessage, MinimumLength = 3)]
        public string Game { get; set; }

        [Display(Name = "Current Activity")] public string Activity { get; set; }

        [Display(Name = "Change Activity")]
        [EnumDataType(typeof(ActivityType))]
        public ActivityType ChangeActivity { get; set; }

        [Required]
        [StringLength(400, ErrorMessage = GlobalConstants.StringLengthErrorMessage, MinimumLength = 3)]
        public string Description { get; set; }
    }
}