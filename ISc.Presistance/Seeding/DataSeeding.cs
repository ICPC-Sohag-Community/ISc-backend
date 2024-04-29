using ISc.Application.Interfaces.Repos;
using ISc.Domain.Comman.Constant;
using ISc.Domain.Comman.Enums;
using ISc.Domain.Models;
using ISc.Domain.Models.IdentityModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ISc.Presistance.Seeding
{
    public class DataSeeding
    {
        public async static void Initialize(IServiceProvider service)
        {
            var dbcontext = service.GetRequiredService<ICPCDbContext>();
            var userManager = service.GetRequiredService<UserManager<Account>>();
            var roleManager = service.GetRequiredService<RoleManager<IdentityRole>>();

            var pendingMigrations = await dbcontext.Database.GetPendingMigrationsAsync();
            var migrations = dbcontext.Database.GetMigrations();

            if (pendingMigrations.Count() == migrations.Count())
            {
                var unitOfWork = service.GetService<IUnitOfWork>();

                await roleManager.CreateAsync(new IdentityRole(Roles.Leader));
                await roleManager.CreateAsync(new IdentityRole(Roles.Head_Of_Camp));
                await roleManager.CreateAsync(new IdentityRole(Roles.Mentor));
                await roleManager.CreateAsync(new IdentityRole(Roles.Trainee));
                await roleManager.CreateAsync(new IdentityRole(Roles.Instructor));

                var leader = new Account
                {
                    FirstName = "ICPC",
                    MiddleName = "Sohag",
                    LastName = "Community",
                    UserName = "ICPCSohag",
                    PhoneNumber = "0112352462",
                    NationalId = "11111111111111",
                    Grade = 4,
                    Email = "icpc.sohag.community@gmail.com",
                    Gender = Gender.male,
                    CodeForceHandle = "IcpcSohag"
                };
                await userManager.CreateAsync(leader, "123654@ICPC#univ");
                await userManager.AddToRoleAsync(leader, Roles.Leader);


                await unitOfWork.Repository<CampModel>().AddAsync(new CampModel { Name = "NewComer" });
                await unitOfWork.Repository<CampModel>().AddAsync(new CampModel { Name = "Phase1" });
                await unitOfWork.Repository<CampModel>().AddAsync(new CampModel { Name = "Phase2" });

                await unitOfWork.SaveAsync();
            }




        }
    }
}
