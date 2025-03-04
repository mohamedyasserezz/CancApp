using CanaApp.Domain.Entities.Models;

namespace CanaApp.Domain.Entities.Comunity
{
    class Comment
    {
        public int Id { get; set; }
        public DateTime Time { get; set; }
        public string Content { get; set; }

        // Foreign keys and navigation properties
        public int PostId { get; set; }
        public Post Post { get; set; }

        public ApplicationUser User { get; set; }
        public string UserId { get; set; }

        
        public ICollection<Reaction>? Reactions = new List<Reaction>();
        public int? ParentCommentId { get; set; }
        public Comment ParentComment { get; set; }
        public ICollection<Comment> ChildComments = new List<Comment>();
    }
}
