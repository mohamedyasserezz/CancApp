using CanaApp.Domain.Entities.Models;

namespace CanaApp.Domain.Entities.Chats
{
    public class Chat
    {
        public int Id { get; set; }
        public string Participant1Id { get; set; } = default!; 
        public string Participant2Id { get; set; } = default!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public ApplicationUser Participant1 { get; set; } = default!;
        public ApplicationUser Participant2 { get; set; } = default!;
        public ICollection<Message> Messages { get; set; } = new List<Message>();

    }
}
