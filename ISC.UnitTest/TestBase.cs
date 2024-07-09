using ISc.Application.Extension;
using ISc.Application.Interfaces;
using ISc.Application.Interfaces.Repos;
using ISc.Domain.Models.CommunityStaff;
using ISc.Presistance.Repos;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace ISC.UnitTests
{
    public class TestBase
    {
        protected IServiceProvider _serviceProvider;

        public TestBase()
        {
            var builder = WebApplication.CreateBuilder();
            builder.Configuration.AddJsonFile("appsettings.json", false, true);

            builder.Services.AddApplication();
            AddRepositories(builder.Services);

            _serviceProvider = builder.Services.BuildServiceProvider();

        }

        private static void AddRepositories(IServiceCollection services)
        {
            services
                .AddTransient(typeof(IUnitOfWork), typeof(UnitOfWorkTest))
                .AddTransient<IStuffArchiveRepo, StuffArhciveRepo>();
        }
    }
}
