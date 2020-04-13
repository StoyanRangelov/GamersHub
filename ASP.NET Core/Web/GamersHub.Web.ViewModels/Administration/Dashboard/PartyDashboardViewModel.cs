using GamersHub.Data.Models;
using GamersHub.Services.Mapping;

namespace GamersHub.Web.ViewModels.Administration.Dashboard
{
    public class PartyDashboardViewModel : IMapFrom<Party>
    {
        public string CreatorUsername { get; set; }

        public int PartyApplicantsCount { get; set; }
    }
}