using CanaApp.Domain.Entities.Comunity;

namespace CanaApp.Domain.Entities.Models
{
    public class Doctor
    {
        public string MedicalSyndicatePhoto { get; set; } = default!;
        public string ImageId { get; set; } = default!;
        public ApplicationUser ApplicationUser { get; set; } = default!;
        public bool IsConfirmedByAdmin { get; set; }
        public bool IsDisabled { get; set; }
        public int NumberOfWarrings { get; set; }
        public string UserId { get; set; }

        // Community
        public ICollection<Post> Posts = new List<Post>();
        public ICollection<Reaction> Reactions = new List<Reaction>();
        public ICollection<Comment> Comments = new List<Comment>(); 
    }
}
