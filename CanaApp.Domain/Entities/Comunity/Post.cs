using CanaApp.Domain.Entities.Models;

namespace CanaApp.Domain.Entities.Comunity
{
    class Post
    {
        public int Id { get; set; }
        public DateTime Time { get; set; }
        public string Content { get; set; }

        // Foreign keys and navigation properties
        public ApplicationUser User { get; set; }
        public string UserId { get; set; }

        public ICollection<Reaction>? Reactions = new List<Reaction>();
        public ICollection<Comment> Comments = new List<Comment>();
    }
}
