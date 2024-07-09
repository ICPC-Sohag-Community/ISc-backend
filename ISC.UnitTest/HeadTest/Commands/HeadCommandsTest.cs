using ISc.Application.Interfaces.Repos;
using ISc.Domain.Models;
using ISc.Presistance.Repos;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISC.UnitTests.HeadTest.Commands
{
    
    public class HeadCommandsTest:TestBase
    {
        public HeadCommandsTest()
        {
            
        }

        [Fact]
        public async Task CreateCampCommand_WhenCreate_ReturnTrue()
        {
            var unitofWork = _serviceProvider.GetRequiredService<IUnitOfWork>();

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
