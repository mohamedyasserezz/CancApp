using CanaApp.Domain.Entities.Models;

namespace CanaApp.Domain.Entities.Records
{
    public class RecordAccess
    {
        public int Id { get; set; }
        public string DoctorId { get; set; } = default!;
        public string PatientId { get; set; } = default!;
        public DateTime RequestedAt { get; set; }
        public bool? IsApproved { get; set; } // null = pending, true = approved, false = rejected
        // Navigation properties
        public ApplicationUser Doctor { get; set; } = default!;
        public ApplicationUser Patient { get; set; } = default!;
    }
}
