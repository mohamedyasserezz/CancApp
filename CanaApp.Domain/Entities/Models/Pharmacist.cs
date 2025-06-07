using CanaApp.Domain.Entities.Comunity;

namespace CanaApp.Domain.Entities.Models
{
    public class Pharmacist
    {
        public string? ImageId { get; set; } 
        public string? ImagePharmacyLicense { get; set; }
        public bool IsConfirmedByAdmin { get; set; }
        public ApplicationUser ApplicationUser { get; set; } = default!;
        public string UserId { get; set; } = default!;
        public bool IsCompletedProfileFailed { get; set; }
        public int NumberOfWorkingHours { get; set; }
        public bool IsDeliveryEnabled { get; set; }


    }
}
