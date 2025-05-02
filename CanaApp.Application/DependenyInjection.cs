using CanaApp.Application.Mapping;
using CanaApp.Application.Services.Authentication;
using CanaApp.Application.Services.Community.Comments;
using CanaApp.Application.Services.Community.Posts;
using CanaApp.Application.Services.Community.Reactions;
using CanaApp.Application.Services.Email;
using CanaApp.Application.Services.Files;
using CanaApp.Domain.Contract.Service.Authentication;
using CanaApp.Domain.Contract.Service.Community.Comment;
using CanaApp.Domain.Contract.Service.Community.Post;
using CanaApp.Domain.Contract.Service.Community.Reaction;
using CanaApp.Domain.Contract.Service.File;
using Hangfire;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CanaApp.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddScoped<IJwtProvider, JwtProvider>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<IEmailSender, EmailService>();


            #region Hangfire
            services.AddHangfire(Configuration => Configuration
             .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
             .UseSimpleAssemblyNameTypeSerializer()
             .UseRecommendedSerializerSettings()
             .UseSqlServerStorage(configuration.GetConnectionString("HangfireConnection")));

            // Add the processing server as IHostedService
            services.AddHangfireServer();
            #endregion

            #region auto mapper

            services.AddHttpContextAccessor();

            // Register AutoMapper with a custom configuration
            services.AddAutoMapper(config =>
            {
                // Explicitly add the MappingProfile with dependency injection
                config.ConstructServicesUsing(type => services.BuildServiceProvider().GetService(type));
                config.AddProfile<MappingProfile>();
            }, typeof(AssemblyInformation).Assembly);


            #endregion

            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<IReactionService, ReactionService>();
            services.AddScoped<IPostService, PostService>();

            #region Caching
            services.AddHybridCache();
            #endregion

            return services;
        }
    }
}
