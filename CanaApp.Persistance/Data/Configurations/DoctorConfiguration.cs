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
                .OnDelete(DeleteBehavior.NoAction);

         

            builder.HasData(
                new Doctor
                {
                    UserId = DefaultUsers.DoctorId,
                    IsConfirmedByAdmin = true,
                    IsCompletedProfileFailed = false,
                    
                });
        }
    }
}
