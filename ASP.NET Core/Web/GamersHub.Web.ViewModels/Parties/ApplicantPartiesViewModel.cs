using System;
using GamersHub.Data.Models;
using GamersHub.Services.Mapping;

namespace GamersHub.Web.ViewModels.Parties
{
    public class ApplicantPartiesViewModel : IMapFrom<PartyApplicant>
    {
        public int PartyId { get; set; }

        public string ApplicantId { get; set; }

        public string PartyCreatorUsername { get; set; }

        public string PartyCreatorDiscordUsername { get; set; }

        public string PartyGame { get; set; }

        public string PartyActivity { get; set; }

        public string PartyDescription { get; set; }

        public DateTime PartyCreatedOn { get; set; }

        public bool PartyIsFull { get; set; }

        public ApplicationStatusType ApplicationStatus { get; set; }
    }
}