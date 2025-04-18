using CanaApp.Domain.Entities.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace CanaApp.Persistance.Data.Configurations
{
    public class PsychiatristConfiguration : IEntityTypeConfiguration<Psychiatrist>
    {
        public void Configure(EntityTypeBuilder<Psychiatrist> builder)
        {

            builder.HasKey(d => d.UserId);

            builder.HasOne(p => p.ApplicationUser)
                .WithMany()
                .HasForeignKey(p => p.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(p => p.Posts)
                .WithOne()
                .HasForeignKey(p => p.UserId);

            builder.HasMany(p => p.Reactions)
                .WithOne()
                .HasForeignKey(r => r.UserId);

            builder.HasMany(p => p.Comments)
                .WithOne()
                .HasForeignKey(c => c.UserId);
        }
    }
}
