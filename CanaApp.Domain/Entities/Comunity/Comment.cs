using CanaApp.Domain.Entities.Models;

namespace CanaApp.Domain.Entities.Comunity
{
    public class Comment
    {
        public int Id { get; set; }
        public DateTime Time { get; set; } = DateTime.UtcNow;       
        public string Content { get; set; } = default!;

        // Foreign keys and navigation properties
        public int PostId { get; set; }
        public Post Post { get; set; } = default!;

        public ApplicationUser User { get; set; } = default!;
        public string UserId { get; set; } = default!;
        public bool IsReported { get; set; }

        public ICollection<Reaction>? Reactions = new List<Reaction>();
    }
}
