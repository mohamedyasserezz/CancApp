using CanaApp.Domain.Contract.Infrastructure;
using CanaApp.Domain.Contract.Service.Notification;
using CanaApp.Persistance.Data;
using CanaApp.Persistance.Services;
using CanaApp.Persistance.UninOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CanaApp.Persistance
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistance(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
                .ConfigureWarnings(w =>
                w.Ignore(RelationalEventId.PendingModelChangesWarning))
                ;
            });


            services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));

            services.AddSingleton<FirebaseService>();
            services.AddSingleton<INotificationServices, FirebaseService>();


            return services;
        }
    }
}
