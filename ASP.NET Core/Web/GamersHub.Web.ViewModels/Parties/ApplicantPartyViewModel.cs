using System.Collections.Generic;
using System.Linq;
using GamersHub.Data.Models;
using GamersHub.Services.Mapping;

namespace GamersHub.Web.ViewModels.Parties
{
    public class ApplicantPartyViewModel : IMapFrom<ApplicationUser>
    {
        public string Username { get; set; }

        public int PagesCount { get; set; }

        public int CurrentPage { get; set; }

        public IEnumerable<ApplicantPartiesViewModel> PartyApplications { get; set; }
    }
}