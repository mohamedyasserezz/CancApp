using CanaApp.Domain.Entities.Models;
using CancApp.Shared.Common.Consts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CanaApp.Persistance.Data.Configurations
{
    public class DoctorConfiguration : IEntityTypeConfiguration<Doctor>
    {
        public void Configure(EntityTypeBuilder<Doctor> builder)
        {

            builder.HasKey(d => d.UserId);



            builder.HasOne(d => d.ApplicationUser)
                .WithMany()
                .HasForeignKey(d => d.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(d => d.Posts)
                .WithOne()
                .HasForeignKey(p => p.UserId);

            builder.HasMany(d => d.Reactions)
                .WithOne()
                .HasForeignKey(r => r.UserId);

            builder.HasMany(d => d.Comments)
                .WithOne()
                .HasForeignKey(c => c.UserId);

            builder.HasData(
                new Doctor
                {
                    UserId = DefaultUsers.DoctorId,
                    IsConfirmedByAdmin = true,
                    IsDisabled = false,
                    NumberOfWarrings = 0,
                    IsCompletedProfileFailed = false,
                    
                });
        }
    }
}
