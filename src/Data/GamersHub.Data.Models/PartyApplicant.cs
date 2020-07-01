namespace GamersHub.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public class PartyApplicant
    {
        public int PartyId { get; set; }

        public virtual Party Party { get; set; }

        [Required]
        public string ApplicantId { get; set; }

        public virtual ApplicationUser Applicant { get; set; }

        public ApplicationStatusType ApplicationStatus { get; set; }
    }
}
