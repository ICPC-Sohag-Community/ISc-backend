using System.Runtime.CompilerServices;
using ISc.Application.Interfaces.Repos;
using ISc.Presistance.Repos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ISc.Presistance.Extension
{
    public static class ServiceCollection
    {
        public static IServiceCollection AddPresistance(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ICPCDbContext>(i => i.UseSqlServer(configuration.GetConnectionString("DataBase")))
                    .AddCollections();
            return services;
        }
        private static IServiceCollection AddCollections(this IServiceCollection services)
        {
            services.AddTransient<IMentorRepo, MentorRepo>()
                    .AddTransient<ITraineeRepo, TraineeRepo>()
                    .AddTransient<IHeadRepo, HeadRepo>()
                    .AddTransient<IStuffArchiveRepo, IStuffArchiveRepo>();

            return services;
        }
    }
}
