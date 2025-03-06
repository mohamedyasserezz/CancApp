using CanaApp.Domain.Entities.Records;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanaApp.Persistance.Data.Configurations
{
    public class RecordConfiguration : IEntityTypeConfiguration<Record>
    {
        public void Configure(EntityTypeBuilder<Record> builder)
        {

            builder.Property(x => x.RecordType)
                .HasConversion(

                    T => T.ToString(),
                    t => (RecordType)System.Enum.Parse(typeof(RecordType), t)
                );

            builder.HasOne(r => r.Patient)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(r => r.Doctor)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
