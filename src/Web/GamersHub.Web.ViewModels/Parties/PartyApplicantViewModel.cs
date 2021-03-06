﻿namespace GamersHub.Web.ViewModels.Parties
{
    using GamersHub.Data.Models;
    using GamersHub.Services.Mapping;

    public class PartyApplicantViewModel : IMapFrom<PartyApplicant>
    {
        public string ApplicantId { get; set; }

        public string ApplicantUsername { get; set; }

        public int PartyId { get; set; }

        public GamingExperienceType ApplicantGamingExperience { get; set; }

        public ApplicationStatusType ApplicationStatus { get; set; }
    }
}
