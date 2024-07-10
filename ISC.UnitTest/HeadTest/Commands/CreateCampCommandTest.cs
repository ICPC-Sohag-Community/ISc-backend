using ISc.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISC.UnitTests.HeadTest.Commands
{
    public class CreateCampCommandTest:TestBase
    {
        [Fact]
        public async Task CreateCampCommand_WhenCreate_ReturnTrue()
        {
            var unitofWork = GetUnitOfWork();
            var usermanager = GetUserManager();

            var users = await usermanager.Users.ToListAsync();
            var camp = new Camp()
            {
                Name = "Abc",
                startDate = DateOnly.MinValue,
                EndDate = DateOnly.MaxValue,
                OpenForRegister = true,
                Term = ISc.Domain.Comman.Enums.Term.Summer,
                DurationInWeeks = 6
            };

            await unitofWork.Repository<Camp>().AddAsync(camp);

            Assert.True(camp.Id > 0);
        }
    }
}
