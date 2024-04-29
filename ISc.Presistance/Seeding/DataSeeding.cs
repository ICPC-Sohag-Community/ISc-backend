using ISc.Application.Interfaces.Repos;
using ISc.Domain.Comman.Enums;
using ISc.Domain.Models;
using ISc.Domain.Models.IdentityModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace ISc.Presistance.Seeding
{
    public  class DataSeeding
    {
        public async static void Initialize(IServiceProvider service)
        {
            var dbcontext = service.GetService<ICPCDbContext>();

             dbcontext.Database.EnsureCreatedAsync().Wait();

             var userManager = service.GetService<UserManager<Account>>();

            var unitOfWork = service.GetService<IUnitOfWork>();

            var roles=new List<IdentityRole>() {
            new IdentityRole(){ Name="Leader", NormalizedName="LEADER",Id=Guid.NewGuid().ToString()},
            new IdentityRole(){ Name="HeadOfCamp", NormalizedName="HeadOfCamp".ToUpper(),Id=Guid.NewGuid().ToString()},
            new IdentityRole(){ Name="Mentor", NormalizedName="Mentor".ToUpper(),Id=Guid.NewGuid().ToString()},
            new IdentityRole(){ Name="Trainee", NormalizedName="Trainee".ToUpper(),Id=Guid.NewGuid().ToString()}
            };

            await dbcontext.Roles.AddRangeAsync(roles);


            var leader = new Account { 
               FirstName="Ahmed",
               MiddleName="Maher",
               LastName="Sebaq",
               UserName="A.Sebaq",
               PhoneNumber="01012345678",
               NationalId="12345678912345",
               VjudgeHandle="A.sebaq",
               Grade=4,
               Email="A.Sebaq@gmail.com",
               Gender=Gender.male,
               CodeForceHandle="A.Sebaq"
            };
            await userManager.CreateAsync(leader, "P@ssw0rd");
            await userManager.AddToRoleAsync(leader, "Leader");
            
           
            await unitOfWork.Repository<CampModel>().AddAsync(new CampModel { Name = "NewComer" });
            await  unitOfWork.Repository<CampModel>().AddAsync(new CampModel { Name = "Phase1" });
            await unitOfWork.Repository<CampModel>().AddAsync(new CampModel { Name = "Phase2" });


        }
    }
}
