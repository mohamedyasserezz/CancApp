using CanaApp.Domain.Entities.Models;

namespace CanaApp.Domain.Entities.Comunity
{
    class Reaction
    {
        public int Id { get; set; }
        public DateTime Time { get; set; }
        public string Type { get; set; } = string.Empty;

        // Foreign keys and navigation properties
        public int PostId { get; set; }
        public Post Post { get; set; } = default!;
        public int CommentId { get; set; }
        public Comment Comment { get; set; } = default!;

        public ApplicationUser ApplicationUser { get; set; } = default!;
        public string UserId { get; set; } = default!;

        public ReactionType ReactionType { get; set; }
    }
}
