using System.Collections.Generic;
using GamersHub.Data.Models;
using GamersHub.Services.Mapping;

namespace GamersHub.Web.ViewModels.Parties
{
    public class PartyHostViewModel : IMapFrom<ApplicationUser>
    {
        public string Username { get; set; }

        public int PagesCount { get; set; }

        public int CurrentPage { get; set; }

        public IEnumerable<PartyWithApplicantsViewModel> UserParties { get; set; }
    }
}