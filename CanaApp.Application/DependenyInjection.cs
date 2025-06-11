using AutoMapper;
using CanaApp.Application.Mapping;
using CanaApp.Application.Mapping.Resolver;
using CanaApp.Application.Services.Authentication;
using CanaApp.Application.Services.Community.Comments;
using CanaApp.Application.Services.Community.Posts;
using CanaApp.Application.Services.Community.Reactions;
using CanaApp.Application.Services.Dashboard;
using CanaApp.Application.Services.Email;
using CanaApp.Application.Services.Files;
using CanaApp.Application.Services.User;
using CanaApp.Domain.Contract.Service.Authentication;
using CanaApp.Domain.Contract.Service.Community.Comment;
using CanaApp.Domain.Contract.Service.Community.Post;
using CanaApp.Domain.Contract.Service.Community.Reaction;
using CanaApp.Domain.Contract.Service.Dashboard;
using CanaApp.Domain.Contract.Service.File;
using CanaApp.Domain.Contract.Service.User;
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


            //services.AddAutoMapper(typeof(MappingProfile));

            //var mapper = services.BuildServiceProvider().GetRequiredService<IMapper>();
            //mapper.ConfigurationProvider.AssertConfigurationIsValid();

            services.AddAutoMapper(config =>
            {
                config.AddProfile<MappingProfile>();
            }, typeof(MappingProfile).Assembly, typeof(CommentProfileResolver).Assembly);

            // Validate AutoMapper configuration
            
            #endregion

            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<IReactionService, ReactionService>();
            services.AddScoped<IPostService, PostService>();
            services.AddScoped<IDashboardServices, DashboardServices>();
            services.AddScoped<IUserServices, UserServices>();

            #region Caching
            services.AddHybridCache();
            #endregion

            return services;
        }
    }
}
