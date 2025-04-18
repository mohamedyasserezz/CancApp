using CanaApp.Domain.Entities.Comunity;

namespace CanaApp.Domain.Entities.Models
{
    public class Pharmacist
    {
        public string ImageId { get; set; } = default!;
        public string ImagePharmacyLicense { get; set; } = default!; public bool IsConfirmedByAdmin { get; set; }
        public ApplicationUser ApplicationUser { get; set; } = default!;
        public string UserId { get; set; } = default!;
        public bool IsDisabled { get; set; }
        public int NumberOfWarrings { get; set; }
        public int NumberOfWorkingHours { get; set; }
        public bool IsDeliveryEnabled { get; set; }


    }
}
