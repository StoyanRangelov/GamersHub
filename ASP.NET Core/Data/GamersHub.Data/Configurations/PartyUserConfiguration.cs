using GamersHub.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GamersHub.Data.Configurations
{
    public class PartyUserConfiguration : IEntityTypeConfiguration<PartyUser>
    {
        public void Configure(EntityTypeBuilder<PartyUser> builder)
        {
            builder
                .HasKey(pu => new {pu.ApplicantId, pu.PartyId});

            builder
                .HasOne(pu => pu.Applicant)
                .WithMany(u => u.PartyApplicants)
                .HasForeignKey(pu => pu.ApplicantId);

            builder
                .HasOne(pu => pu.Party)
                .WithMany(p => p.PartyApplicants)
                .HasForeignKey(p => p.PartyId);
        }
    }
}