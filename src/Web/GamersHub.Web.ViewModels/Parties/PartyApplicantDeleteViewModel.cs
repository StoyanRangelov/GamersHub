namespace GamersHub.Web.ViewModels.Parties
{
    using GamersHub.Data.Models;
    using GamersHub.Services.Mapping;

    public class PartyApplicantDeleteViewModel : IMapFrom<PartyApplicant>
    {
        public string ApplicantUsername { get; set; }

        public ApplicationStatusType ApplicationStatus { get; set; }
    }
}
