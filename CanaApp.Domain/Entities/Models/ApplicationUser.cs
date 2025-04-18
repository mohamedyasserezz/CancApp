using Microsoft.AspNetCore.Identity;

namespace CanaApp.Domain.Entities.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; } = default!;
        public string Address { get; set; } = default!;
        public string? Image { get; set; } = default!;
        public UserType UserType { get; set; }
        public bool IsDisabled { get; set; }
        public int NumberOfWarrings { get; set; }
        public List<RefreshToken> RefreshTokens { get; set; } = [];
    }
}
