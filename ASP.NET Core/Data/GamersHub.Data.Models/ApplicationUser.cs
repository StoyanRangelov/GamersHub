// ReSharper disable VirtualMemberCallInConstructor

using System.ComponentModel.DataAnnotations;

namespace GamersHub.Data.Models
{
    using System;
    using System.Collections.Generic;
    using GamersHub.Data.Common.Models;
    using Microsoft.AspNetCore.Identity;

    public class ApplicationUser : IdentityUser, IAuditInfo, IDeletableEntity
    {
        public ApplicationUser()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Roles = new HashSet<IdentityUserRole<string>>();
            this.Claims = new HashSet<IdentityUserClaim<string>>();
            this.Logins = new HashSet<IdentityUserLogin<string>>();
            this.Posts = new HashSet<Post>();
            this.Replies = new HashSet<Reply>();
            this.Reviews = new HashSet<Review>();
            this.Parties = new HashSet<Party>();
            this.PartyApplicants = new HashSet<PartyApplicant>();
        }

        // Audit info
        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        // Deletable entity
        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public GamingExperienceType GamingExperience { get; set; }

        [Required]
        public string DiscordUsername { get; set; }

        [Required]
        public string ImgUrl { get; set; }

        public virtual ICollection<IdentityUserRole<string>> Roles { get; set; }

        public virtual ICollection<IdentityUserClaim<string>> Claims { get; set; }

        public virtual ICollection<IdentityUserLogin<string>> Logins { get; set; }

        public virtual ICollection<Post> Posts { get; set; }

        public virtual ICollection<Reply> Replies { get; set; }

        public virtual ICollection<Review> Reviews { get; set; }

        public virtual ICollection<Party> Parties { get; set; }

        public virtual ICollection<PartyApplicant> PartyApplicants { get; set; }
    }
}