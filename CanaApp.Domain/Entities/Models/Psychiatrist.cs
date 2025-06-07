using CanaApp.Domain.Entities.Comunity;

namespace CanaApp.Domain.Entities.Models
{
    public class Psychiatrist
    {
        public string? MedicalSyndicatePhoto { get; set; } 
        public string? ImageId { get; set; }
        public ApplicationUser ApplicationUser { get; set; } = default!;
        public string UserId { get; set; } = default!;
        public bool IsCompletedProfileFailed { get; set; }
        public bool IsConfirmedByAdmin { get; set; }

    }
}
