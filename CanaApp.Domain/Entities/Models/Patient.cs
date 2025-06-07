using CanaApp.Domain.Entities.Comunity;

namespace CanaApp.Domain.Entities.Models
{
    public class Patient
    {
        public ApplicationUser ApplicationUser { get; set; } = default!;
        public string UserId { get; set; } = default!;

    }
}
