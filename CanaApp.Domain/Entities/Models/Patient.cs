using CanaApp.Domain.Entities.Comunity;

namespace CanaApp.Domain.Entities.Models
{
    class Patient
    {
        public ApplicationUser ApplicationUser { get; set; } = default!;
        public string UserId { get; set; } = default!;

        // Community
        public ICollection<Post> Posts = new List<Post>();
        public ICollection<Reaction> Reactions = new List<Reaction>();
        public ICollection<Comment> Comments = new List<Comment>();
    }
}
