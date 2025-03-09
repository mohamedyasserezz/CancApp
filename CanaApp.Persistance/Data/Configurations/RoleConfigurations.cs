using CanaApp.Domain.Entities.Roles;
using CancApp.Shared.Common.Consts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CanaApp.Persistance.Data.Configurations
{
    public class RoleConfigurations : IEntityTypeConfiguration<ApplicationRole>
    {
        public void Configure(EntityTypeBuilder<ApplicationRole> builder)
        {
            builder.HasData([
                new ApplicationRole {
                    Id = DefaultRoles.AdminRoleId,
                    Name = DefaultRoles.Admin,
                    NormalizedName = DefaultRoles.Admin.ToUpper(),
                    ConcurrencyStamp = DefaultRoles.AdminRoleConcurrencyStamp,
                },
                new ApplicationRole {
                    Id = DefaultRoles.PatientRoleId,
                    Name = DefaultRoles.Patient,
                    NormalizedName = DefaultRoles.Patient.ToUpper(),
                    ConcurrencyStamp = DefaultRoles.PatientRoleConcurrencyStamp,
                    IsDefault = true,
                },
                new ApplicationRole {
                    Id = DefaultRoles.DoctorRoleId,
                    Name = DefaultRoles.Doctor,
                    NormalizedName = DefaultRoles.Doctor.ToUpper(),
                    ConcurrencyStamp = DefaultRoles.DoctorRoleConcurrencyStamp,
                },
                new ApplicationRole {
                    Id = DefaultRoles.PharmacistRoleId,
                    Name = DefaultRoles.Pharmacist,
                    NormalizedName = DefaultRoles.Pharmacist.ToUpper(),
                    ConcurrencyStamp = DefaultRoles.PharmacistRoleConcurrencyStamp,
                },
                new ApplicationRole {
                    Id = DefaultRoles.PsychiatristRoleId,
                    Name = DefaultRoles.Psychiatrist,
                    NormalizedName = DefaultRoles.Psychiatrist.ToUpper(),
                    ConcurrencyStamp = DefaultRoles.PsychiatristRoleConcurrencyStamp,
                },
                new ApplicationRole {
                    Id = DefaultRoles.VolunteerRoleId,
                    Name = DefaultRoles.Volunteer,
                    NormalizedName = DefaultRoles.Volunteer.ToUpper(),
                    ConcurrencyStamp = DefaultRoles.VolunteerRoleConcurrencyStamp,
                }
                ]);
        }
    }
}
