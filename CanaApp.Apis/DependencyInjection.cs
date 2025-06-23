using CanaApp.Application.Health;
using CanaApp.Application.Services.Authentication;
using CanaApp.Domain.Contract.Service.Authentication;
using CanaApp.Domain.Entities.Models;
using CanaApp.Domain.Entities.Roles;
using CanaApp.Persistance.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace CanaApp.Apis
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApis(this IServiceCollection services, IConfiguration configuration)
        {
            #region Identity
            services.AddControllers();

            services
              .AddIdentity<ApplicationUser, ApplicationRole>()
              .AddEntityFrameworkStores<ApplicationDbContext>()
              .AddDefaultTokenProviders();

            services
                .Configure<IdentityOptions>(options =>
                {
                    options.Password.RequiredLength = 6;
                    options.SignIn.RequireConfirmedEmail = true;
                    options.User.RequireUniqueEmail = true;

                });

            #endregion

            #region JWT
            services.AddSingleton<IJwtProvider, JwtProvider>();

            services.AddOptions<JwtOptions>()
            .BindConfiguration(JwtOptions.SectionName)
                .ValidateDataAnnotations()
                .ValidateOnStart();


            var jwtSettings = configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
               .AddJwtBearer(options =>
               {
                   options.SaveToken = true;
                   options.TokenValidationParameters = new TokenValidationParameters
                   {
                       ValidateIssuerSigningKey = true,
                       ValidateIssuer = true,
                       ValidateAudience = true,
                       ValidateLifetime = true,
                       IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings?.Key!)),
                       ValidIssuer = jwtSettings?.Issuer,
                       ValidAudience = jwtSettings?.Audience
                   };
               });
            #endregion

            #region CORS
            // var allowedOrgins = configuration.GetSection("AllowedOrgins").Get<string[]>();

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder
                        .AllowAnyOrigin()    // Allow requests from any origin
                        .AllowAnyHeader()    // Allow any headers
                        .AllowAnyMethod();   // Allow any HTTP methods (GET, POST, etc.)
                });
            });

            #endregion

            #region SignalR
            services.AddSignalR();
            #endregion

            #region HealthCheck
            services.AddHealthChecks()
                    .AddSqlServer(configuration.GetConnectionString("DefaultConnection")!, name: "database")
                    .AddHangfire(options =>
                    {
                        options.MinimumAvailableServers = 1;
                    })
                    .AddCheck<MailProviderHealthCheck>("mail provider"); 
            #endregion

            return services;
        }
    }
}
