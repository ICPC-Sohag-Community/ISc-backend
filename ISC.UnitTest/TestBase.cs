using ISc.Application.Interfaces;
using ISc.Application.Interfaces.Repos;
using ISc.Domain.Models.IdentityModels;
using ISc.Infrastructure.Services.Media;
using ISc.Presistance;
using ISc.Presistance.Repos;
using Mapster;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Reflection;

namespace ISC.UnitTests
{
    public class TestBase : IDisposable
    {
        protected readonly DbContextOptions<ICPCDbContext> _dbContextOptions;
        protected readonly ServiceProvider _serviceProvider;
        private readonly IServiceScope _serviceScope;

        public TestBase()
        {

            _dbContextOptions = new DbContextOptionsBuilder<ICPCDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

            var services = new ServiceCollection();

            var mockWebHostEnvironment = new Mock<IWebHostEnvironment>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockMediator = new Mock<IMediator>();

            AddServices(services, mockWebHostEnvironment, mockConfiguration,mockMediator);

            _serviceProvider = services.BuildServiceProvider();
            _serviceScope = _serviceProvider.CreateScope();

        }

        private void AddServices(
            ServiceCollection services,
            Mock<IWebHostEnvironment> mockWebHostEnvironment,
            Mock<IConfiguration> mockConfiguration,
            Mock<IMediator> mockMediator)
        {
            services.AddSingleton(provider =>
            {
                return new ICPCDbContext(_dbContextOptions);
            });

            services.AddSingleton(provider =>
            {
                var dbContext = provider.GetRequiredService<ICPCDbContext>();
                var userStore = new UserStore<Account>(dbContext);
                return new UserManager<Account>(userStore, null, null, null, null, null, null, null, null);
            });

            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IStuffArchiveRepo, StuffArhciveRepo>();
            services.AddTransient<IMapper>(provider =>
            {
                var config = new TypeAdapterConfig();
                return new Mapper(config);
            });
            services.AddTransient<IMediaServices, MediaServices>();
            services.AddSingleton(mockWebHostEnvironment.Object);
            services.AddSingleton(mockConfiguration.Object);
            services.AddSingleton(mockMediator.Object);
        }

        public UserManager<Account> GetUserManager()
        {
            return _serviceProvider.GetRequiredService<UserManager<Account>>();
        }

        public ICPCDbContext GetDbContext()
        {
            return _serviceProvider.GetRequiredService<ICPCDbContext>();
        }

        public IUnitOfWork GetUnitOfWork()
        {
            return _serviceProvider.GetRequiredService<IUnitOfWork>();
        }

        public void Dispose()
        {
            using (var serviceScope = _serviceProvider.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<ICPCDbContext>();
                context.Database.EnsureDeleted(); 
            }
        }
    }
}
