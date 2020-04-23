namespace GamersHub.Web.ViewModels.Parties
{
    using System.Collections.Generic;
    using System.Linq;

    using GamersHub.Data.Models;
    using GamersHub.Services.Mapping;

    public class ApplicantPartyViewModel : IMapFrom<ApplicationUser>
    {
        public string Username { get; set; }

        public int PagesCount { get; set; }

        public int CurrentPage { get; set; }

        public IEnumerable<ApplicantPartiesViewModel> PartyApplications { get; set; }
    }
}
