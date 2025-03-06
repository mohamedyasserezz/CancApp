using CanaApp.Domain.Entities.Reminders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CanaApp.Persistance.Data.Configurations
{
    public class VisitReminderConfiguration : IEntityTypeConfiguration<VisitReminder>
    {
        public void Configure(EntityTypeBuilder<VisitReminder> builder)
        {



            builder.HasOne(vr => vr.Patient)
                .WithMany()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
