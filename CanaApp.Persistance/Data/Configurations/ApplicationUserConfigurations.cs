using CanaApp.Domain.Entities.Models;
using CancApp.Shared.Common.Consts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CanaApp.Persistance.Data.Configurations
{
    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.ToTable("Users");

            builder.Property(x => x.FullName)
                .HasMaxLength(100);

            builder.Property(u => u.Address)
                .HasMaxLength(200);

            builder.Property(u => u.Image)
                .HasMaxLength(500);


            builder.Property(x => x.UserType)
                .HasConversion(

                    T => T.ToString(),
                    t => (UserType)System.Enum.Parse(typeof(UserType), t)
                );

            builder.OwnsMany(U => U.RefreshTokens)
                .ToTable("RefreshTokens")
                .WithOwner()
                .HasForeignKey("UserId");

            var passwordHasher = new PasswordHasher<ApplicationUser>();

            builder.HasData([
                new ApplicationUser{
                    Id = DefaultUsers.AdminId,
                    Email = DefaultUsers.AdminEmail,
                    NormalizedEmail = DefaultUsers.AdminEmail.ToUpper(),
                    UserName = DefaultUsers.AdminrUserName,
                    NormalizedUserName = DefaultUsers.AdminrUserName.ToUpper(),
                    Address = DefaultUsers.AdminAddress,
                    FullName = DefaultUsers.AdminName,
                    ConcurrencyStamp = DefaultUsers.AdminConcurrencyStamp,
                    EmailConfirmed = true,
                    UserType = UserType.Admin,
                    SecurityStamp = DefaultUsers.AdminSecurityStamp,
                    PasswordHash = passwordHasher.HashPassword(null!, DefaultUsers.AdminPassword)
                },
                new ApplicationUser{
                    Id = DefaultUsers.DoctorId,
                    Email = DefaultUsers.DoctorEmail,
                    NormalizedEmail = DefaultUsers.DoctorEmail.ToUpper(),
                    UserName = DefaultUsers.DoctorUserName,
                    NormalizedUserName = DefaultUsers.DoctorUserName.ToUpper(),
                    Address = DefaultUsers.DoctorAddress,
                    FullName = DefaultUsers.DoctorName,
                    ConcurrencyStamp = DefaultUsers.DoctorConcurrencyStamp,
                    EmailConfirmed = true,
                    UserType = UserType.Doctor,
                    SecurityStamp = DefaultUsers.DoctorSecurityStamp,
                    PasswordHash = passwordHasher.HashPassword(null!, DefaultUsers.DoctorPassword)
                },
                new ApplicationUser{
                    Id = DefaultUsers.PatientId,
                    Email = DefaultUsers.PatientEmail,
                    NormalizedEmail = DefaultUsers.PatientEmail.ToUpper(),
                    UserName = DefaultUsers.PatientUserName,
                    NormalizedUserName = DefaultUsers.PatientUserName.ToUpper(),
                    Address = DefaultUsers.PatientAddress,
                    FullName = DefaultUsers.PatientName,
                    ConcurrencyStamp = DefaultUsers.PatientConcurrencyStamp,
                    EmailConfirmed = true,
                    UserType = UserType.Patient,
                    SecurityStamp = DefaultUsers.PatientSecurityStamp,
                    PasswordHash = passwordHasher.HashPassword(null!, DefaultUsers.PatientPassword)
                }]
                );
        }
    }
}
