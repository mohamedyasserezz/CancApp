using CanaApp.Domain.Entities.Records;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CanaApp.Persistance.Data.Configurations
{
    public class RecordAccessConfiguration : IEntityTypeConfiguration<RecordAccess>
    {
        public void Configure(EntityTypeBuilder<RecordAccess> builder)
        {
            builder.HasOne(r => r.Doctor)
            .WithMany() 
            .HasForeignKey(r => r.DoctorId)
            .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(r => r.Patient)
                .WithMany() 
                .HasForeignKey(r => r.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(r => r.DoctorId).IsRequired();

            builder.Property(r => r.PatientId).IsRequired();

            builder.Property(r => r.RequestedAt).IsRequired();
        }
    }
}
