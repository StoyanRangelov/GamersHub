namespace GamersHub.Web.ViewModels.Administration.Dashboard
{
    using GamersHub.Data.Models;
    using GamersHub.Services.Mapping;

    public class PartyDashboardViewModel : IMapFrom<Party>
    {
        public string CreatorUsername { get; set; }

        public int PartyApplicantsCount { get; set; }
    }
}
