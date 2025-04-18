using CanaApp.Domain.Entities.Models;
using CancApp.Shared.Common.Consts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CanaApp.Persistance.Data.Configurations
{
    public class PatientConfiguration : IEntityTypeConfiguration<Patient>
    {
        public void Configure(EntityTypeBuilder<Patient> builder)
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

            builder.HasData(
                new Patient
                {
                    UserId = DefaultUsers.PatientId,
                    IsDisabled = false,
                    NumberOfWarrings = 0,
                }
                );
        }
    }
}
