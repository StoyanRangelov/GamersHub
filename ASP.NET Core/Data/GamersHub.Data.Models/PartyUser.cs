using System.ComponentModel.DataAnnotations;

namespace GamersHub.Data.Models
{
    public class PartyUser
    {
        public int PartyId { get; set; }

        public virtual Party Party { get; set; }

        [Required]
        public string ApplicantId { get; set; }

        public virtual ApplicationUser Applicant { get; set; }

        public bool IsApproved { get; set; }
    }
}