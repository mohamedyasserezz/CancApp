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
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
