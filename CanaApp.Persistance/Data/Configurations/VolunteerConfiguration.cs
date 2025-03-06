using CanaApp.Domain.Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CanaApp.Persistance.Data.Configurations
{
    public class VolunteerConfiguration : IEntityTypeConfiguration<Volunteer>
    {
        public void Configure(EntityTypeBuilder<Volunteer> builder)
        {

            builder.HasKey(d => d.UserId);

            builder.HasOne(v => v.ApplicationUser)
                .WithMany()
                .HasForeignKey(v => v.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(v => v.Posts)
                .WithOne()
                .HasForeignKey(p => p.UserId);

            builder.HasMany(v => v.Reactions)
                .WithOne()
                .HasForeignKey(r => r.UserId);

            builder.HasMany(v => v.Comments)
                .WithOne()
                .HasForeignKey(c => c.UserId);
        }
    }
}
