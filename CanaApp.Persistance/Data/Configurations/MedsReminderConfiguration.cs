using CanaApp.Domain.Entities.Reminders;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanaApp.Persistance.Data.Configurations
{
    public class MedsReminderConfiguration : IEntityTypeConfiguration<MedsReminder>
    {
        public void Configure(EntityTypeBuilder<MedsReminder> builder)
        {

            builder.Property(mr => mr.Frequency)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.MedsReminderTypes)
                .HasConversion(

                    T => T.ToString(),
                    t => (MedsReminderTypes)System.Enum.Parse(typeof(MedsReminderTypes), t)
                );

            builder.HasOne(mr => mr.Patient)
                .WithMany()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
