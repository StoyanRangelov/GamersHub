using GamersHub.Data.Models;
using GamersHub.Services.Mapping;

namespace GamersHub.Web.ViewModels.Parties
{
    public class PartyApplicantViewModel : IMapFrom<PartyApplicant>
    {
        public string ApplicantId { get; set; }

        public string ApplicantUsername { get; set; }

        public int PartyId { get; set; }

        public GamingExperienceType ApplicantGamingExperience { get; set; }

        public bool IsApproved { get; set; }

        public bool IsDeclined { get; set; }
    }
}