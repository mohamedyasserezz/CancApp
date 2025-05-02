using CanaApp.Domain.Entities.Comunity;

namespace CanaApp.Domain.Entities.Models
{
    public class Patient
    {
        public ApplicationUser ApplicationUser { get; set; } = default!;
        public bool IsDisabled { get; set; }
        public int NumberOfWarrings { get; set; }
        public string UserId { get; set; } = default!;

    }
}
