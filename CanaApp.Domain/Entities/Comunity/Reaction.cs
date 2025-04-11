using CanaApp.Domain.Entities.Models;

namespace CanaApp.Domain.Entities.Comunity
{
    public class Reaction
    {
        public int Id { get; set; }
        public DateTime Time { get; set; } = DateTime.UtcNow;

        // Foreign keys and navigation properties
        public int PostId { get; set; }
        public Post Post { get; set; } = default!;
        public int? CommentId { get; set; }
        public Comment? Comment { get; set; } = default!;

        public ApplicationUser ApplicationUser { get; set; } = default!;
        public string UserId { get; set; } = default!;

        public ReactionType ReactionType { get; set; }
    }
}
