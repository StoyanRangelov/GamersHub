namespace GamersHub.Data.Configurations
{
    using GamersHub.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class PartyApplicantConfiguration : IEntityTypeConfiguration<PartyApplicant>
    {
        public void Configure(EntityTypeBuilder<PartyApplicant> builder)
        {
            builder
                .HasKey(pu => new { pu.ApplicantId, pu.PartyId });

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
