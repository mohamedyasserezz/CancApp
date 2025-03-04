using CanaApp.Domain.Entities.Models;
using Microsoft.AspNetCore.Identity;

namespace CanaApp.Apis.Extentions
{
    public static class IdentityExtentions
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));

            //services.AddIdentity<ApplicationUser, IdentityRole>();
            services.AddIdentity<ApplicationUser, IdentityRole>(identityOptions =>
            {
                identityOptions.User.RequireUniqueEmail = true;

                identityOptions.SignIn.RequireConfirmedAccount = true;
                identityOptions.SignIn.RequireConfirmedEmail = true;
                identityOptions.SignIn.RequireConfirmedPhoneNumber = true;


                /// identityOptions.Password.RequireNonAlphanumeric = true;
                /// identityOptions.Password.RequiredUniqueChars = 2;
                /// identityOptions.Password.RequiredLength = 6;
                /// identityOptions.Password.RequireDigit = true;
                /// identityOptions.Password.RequireLowercase = true;
                /// identityOptions.Password.RequireUppercase = true;


                identityOptions.Lockout.AllowedForNewUsers = true;
                identityOptions.Lockout.MaxFailedAccessAttempts = 10;
                identityOptions.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);


                //identityOptions.Stores
                //identityOptions.ClaimsIdentity
                //identityOptions.Tokens
            })
                .AddEntityFrameworkStores<StoreIdentityDbContext>();

            services.AddScoped(typeof(Func<IAuthService>), servicesProvider =>
            {
                return () => servicesProvider.GetRequiredService<IAuthService>();
            });

            //services.AddAuthentication();
            //services.AddAuthentication("Hamada");
            services.AddAuthentication(authenticationConfigureOptions =>
            {
                authenticationConfigureOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                authenticationConfigureOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(configureOptions =>
                {
                    configureOptions.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateAudience = true,
                        ValidateIssuer = true,
                        ValidateIssuerSigningKey = true,
                        ValidateLifetime = true,

                        ClockSkew = TimeSpan.FromMinutes(0),
                        ValidIssuer = configuration["JwtSettings:Issuer"],
                        ValidAudience = configuration["JwtSettings:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:Key"]!)),
                    };
                });

            return services;
        }
    }
}
