using CancApp.Shared.Common.Consts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CanaApp.Persistance.Data.Configurations
{
    public class UserRoleConfigurations : IEntityTypeConfiguration<IdentityUserRole<string>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
        {
            builder.HasData([
                new IdentityUserRole<string>{
                    RoleId = DefaultRoles.AdminRoleId,
                    UserId = DefaultUsers.AdminId,
                },
                new IdentityUserRole<string>{
                    RoleId = DefaultRoles.PatientRoleId,
                    UserId = DefaultUsers.PatientId,
                },
                new IdentityUserRole<string>{
                    RoleId = DefaultRoles.DoctorRoleId,
                    UserId = DefaultUsers.DoctorId,
                }]);
        }
    }
}
