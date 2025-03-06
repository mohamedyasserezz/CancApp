using Microsoft.AspNetCore.Identity;

namespace CanaApp.Domain.Entities.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; } = default!;
        public string Address { get; set; } = default!;
        public string? Image { get; set; } = default!;
        public UserType UserType { get; set; }
        public List<RefreshToken> RefreshTokens { get; set; } = [];
    }
}
