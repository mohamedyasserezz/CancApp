using CanaApp.Domain.Entities.Models;

namespace CanaApp.Domain.Entities.Chats
{
    public class Message
    {
        public int Id { get; set; }
        public string SenderId { get; set; } = default!;
        public string? AttachmentUrl { get; set; }
        public string Content { get; set; } = default!;
        public DateTime SentAt { get; set; } = DateTime.UtcNow;
        public bool IsRead { get; set; } // Track read status

        // Navigation properties
        public ApplicationUser Sender { get; set; } = default!;
        public Chat Chat { get; set; } = default!;
    }

}
