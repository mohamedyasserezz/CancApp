using CanaApp.Domain.Entities.Models;

namespace CanaApp.Domain.Entities.Chats
{
    class Message
    {
        public int Id { get; set; }
        public string SenderId { get; set; } // ApplicationUser ID
        public string? AttachmentUrl { get; set; }
        public string Content { get; set; }
        public DateTime SentAt { get; set; } = DateTime.UtcNow;
        public bool IsRead { get; set; } // Track read status

        // Navigation properties
        public ApplicationUser Sender { get; set; }
        public Chat Chat { get; set; }
    }

}
