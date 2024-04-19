using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ISc.Presistance.Extension
{
    public static class ServiceCollection
    {
        public static IServiceCollection AddPresistance(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ICPCDbContext>(i => i.UseSqlServer(configuration.GetConnectionString("DataBase")));
            return services;
        }
       
    }
}
