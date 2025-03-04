using CanaApp.Domain.Entities.Comunity;

namespace CanaApp.Domain.Entities.Models
{
    public class Pharmacist
    {
        public string LicenseNumber { get; set; } = default!;
        public ApplicationUser ApplicationUser { get; set; } = default!;
        public string UserId { get; set; } = default!;

      
    }
}
