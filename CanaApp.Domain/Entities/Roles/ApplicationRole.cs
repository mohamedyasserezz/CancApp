using Microsoft.AspNetCore.Identity;

namespace CanaApp.Domain.Entities.Roles
{
    public class ApplicationRole : IdentityRole
    {
        public bool IsDefault { get; set; }
        public bool IsDeleted { get; set; }
    }
}
