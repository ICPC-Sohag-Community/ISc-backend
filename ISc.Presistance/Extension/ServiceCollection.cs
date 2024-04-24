using ISc.Application.Interfaces.Repos;
using ISc.Domain.Models.IdentityModels;
using ISc.Presistance.Repos;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ISc.Presistance.Extension
{
    public static class ServiceCollection
    {
        public static IServiceCollection AddPresistance(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddContext(configuration)
                    .AddCollections();
            return services;
        }
        private static IServiceCollection AddContext(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DataBase");

            services.AddDbContext<ICPCDbContext>(options =>
               options.UseLazyLoadingProxies().UseSqlServer(connectionString,
                   builder => builder.MigrationsAssembly(typeof(ICPCDbContext).Assembly.FullName)));

            // Identity configuration
            services.AddIdentity<Account, IdentityRole>()
                    .AddEntityFrameworkStores<ICPCDbContext>()
                    .AddUserManager<UserManager<Account>>()
                    .AddRoleManager<RoleManager<IdentityRole>>()
                    .AddSignInManager()
                    .AddDefaultTokenProviders();


            return services;
        }
        private static IServiceCollection AddCollections(this IServiceCollection services)
        {
            services.AddTransient<IStuffArchiveRepo, StuffArhciveRepo>();

            return services;
        }
    }
}
