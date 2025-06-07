using CanaApp.Domain.Entities.Comunity;

namespace CanaApp.Domain.Entities.Models
{
    public class Volunteer
    {
        public ApplicationUser ApplicationUser { get; set; } = default!;
        public string UserId { get; set; } = default!;
    }
}
