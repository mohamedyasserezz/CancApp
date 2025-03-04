using CanaApp.Domain.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanaApp.Domain.Entities.Chats
{
    class Chat
    {
        public int Id { get; set; }
        public string Participant1Id { get; set; } // ApplicationUser ID (e.g., Patient)
        public string Participant2Id { get; set; } // ApplicationUser ID (e.g., Doctor)
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public ApplicationUser Participant1 { get; set; }
        public ApplicationUser Participant2 { get; set; }
        public ICollection<Message> Messages { get; set; } = new List<Message>();

    }
}
