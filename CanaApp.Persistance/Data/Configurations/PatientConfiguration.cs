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
                .OnDelete(DeleteBehavior.NoAction);

     

            builder.HasData(
                new Patient
                {
                    UserId = DefaultUsers.PatientId,
                }
                );
        }
    }
}
