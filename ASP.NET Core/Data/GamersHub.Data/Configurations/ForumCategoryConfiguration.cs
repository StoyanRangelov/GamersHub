namespace GamersHub.Data.Configurations
{
    using GamersHub.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class ForumCategoryConfiguration : IEntityTypeConfiguration<ForumCategory>
    {
        public void Configure(EntityTypeBuilder<ForumCategory> builder)
        {
            builder
                .HasKey(fc => new { fc.ForumId, fc.CategoryId });

            builder
                .HasOne(fc => fc.Forum)
                .WithMany(f => f.ForumCategories)
                .HasForeignKey(fc => fc.ForumId);

            builder
                .HasOne(fc => fc.Category)
                .WithMany(c => c.CategoryForums)
                .HasForeignKey(fc => fc.CategoryId);
        }
    }
}
