using ISc.Application.Interfaces;
using ISc.Application.Interfaces.Repos;
using ISc.Domain.Comman.Constant;
using ISc.Domain.Comman.Enums;
using ISc.Domain.Models.IdentityModels;
using ISc.Infrastructure.Services.Media;
using ISc.Presistance;
using ISc.Presistance.Repos;
using ISc.UnitTests.FakesOjbects;
using Mapster;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Reflection;

namespace ISc.UnitTests
{
    public class TestBase : IDisposable
    {
        protected readonly DbContextOptions<ICPCDbContext> _dbContextOptions;
        protected readonly ServiceProvider _serviceProvider;
        private readonly IServiceScope _serviceScope;
        private readonly IServiceScopeFactory _scopeFactory;

        public TestBase()
        {

            _dbContextOptions = new DbContextOptionsBuilder<ICPCDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

            var services = new ServiceCollection();

            var mockWebHostEnvironment = new Mock<IWebHostEnvironment>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockMediator = new Mock<IMediator>();


            AddServices(services, mockWebHostEnvironment, mockConfiguration, mockMediator);

            _serviceProvider = services.BuildServiceProvider();
            _serviceScope = _serviceProvider.CreateScope();
            _scopeFactory = _serviceProvider.GetRequiredService<IServiceScopeFactory>();

            InitializeDatabase().GetAwaiter();
        }

        private void AddServices(
            ServiceCollection services,
            Mock<IWebHostEnvironment> mockWebHostEnvironment,
            Mock<IConfiguration> mockConfiguration,
            Mock<IMediator> mockMediator)
        {
            services.AddIdentity<Account, IdentityRole>()
               .AddEntityFrameworkStores<ICPCDbContext>()
               .AddDefaultTokenProviders();

            services.AddSingleton(provider =>
            {
                return new ICPCDbContext(_dbContextOptions);
            });


            services.AddLogging(builder =>
            {
                builder.AddConsole();
                builder.AddDebug();
            });

            var config = TypeAdapterConfig.GlobalSettings;
            config.Scan(Assembly.GetAssembly(new Application.Comman.Mapping.MentorMapping().GetType())!);
            services.AddSingleton(config);

            services.AddScoped<IMapper, ServiceMapper>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IStuffArchiveRepo, StuffArhciveRepo>();
            services.AddTransient<IMediaServices, MediaServices>();
            services.AddSingleton(mockWebHostEnvironment.Object);
            services.AddSingleton(mockConfiguration.Object);
            services.AddSingleton(mockMediator.Object);
        }

        public UserManager<Account> GetUserManager()
        {
            return _serviceProvider.GetRequiredService<UserManager<Account>>();
        }

        public RoleManager<IdentityRole> GetRoleManager()
        {
            return _serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        }

        public ICPCDbContext GetDbContext()
        {
            return _serviceProvider.GetRequiredService<ICPCDbContext>();
        }

        public IUnitOfWork GetUnitOfWork()
        {
            return _serviceProvider.GetRequiredService<IUnitOfWork>();
        }

        public async Task<Account> CreateUserAsync()
        {
            var userManager = GetUserManager();

            var user = new FakeAccount().Generate();

            await userManager.CreateAsync(user);

            return user;
        }

        public void Dispose()
        {
            var context = _serviceProvider.GetRequiredService<ICPCDbContext>();
            context.Database.EnsureDeleted();
        }
        private async Task InitializeDatabase()
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ICPCDbContext>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                await context.Database.EnsureCreatedAsync();

                await roleManager.CreateAsync(new IdentityRole(Roles.Leader));
                await roleManager.CreateAsync(new IdentityRole(Roles.Head_Of_Camp));
                await roleManager.CreateAsync(new IdentityRole(Roles.Mentor));
                await roleManager.CreateAsync(new IdentityRole(Roles.Trainee));
                await roleManager.CreateAsync(new IdentityRole(Roles.Instructor));
            }
        }

    }
}
