using CanaApp.Domain.Contract.Infrastructure;
using CanaApp.Persistance.Data;
using CanaApp.Persistance.UninOfWork;
using Microsoft.EntityFrameworkCore;
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
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });


            services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));


            return services;
        }
    }
}
