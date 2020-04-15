using GamersHub.Data.Models;
using GamersHub.Services.Mapping;

namespace GamersHub.Web.ViewModels.Parties
{
    public class PartyApplicantDeleteViewModel : IMapFrom<PartyApplicant>
    {
        public string ApplicantUsername { get; set; }

        public ApplicationStatusType ApplicationStatus { get; set; }
    }
}