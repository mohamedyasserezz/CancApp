using CanaApp.Domain.Entities.Chats;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanaApp.Persistance.Data.Configurations
{
    public class ChatConfiguration : IEntityTypeConfiguration<Chat>
    {
        public void Configure(EntityTypeBuilder<Chat> builder)
        {

            builder.HasOne(c => c.Participant1)
                .WithMany()
                .HasForeignKey(c => c.Participant1Id)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(c => c.Participant2)
                .WithMany()
                .HasForeignKey(c => c.Participant2Id)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(c => c.Messages)
                .WithOne(m => m.Chat)
                .HasForeignKey(m => m.Id)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
