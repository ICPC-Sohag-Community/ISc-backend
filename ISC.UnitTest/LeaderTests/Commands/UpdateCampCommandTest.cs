using FluentAssertions;
using ISc.Application.Features.Leader.Camps.Commands.Update;
using ISc.Domain.Comman.Constant;
using ISc.Domain.Models;
using ISc.Domain.Models.CommunityStaff;
using ISc.UnitTests.FakesObjects;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace ISc.UnitTests.LeaderTests.Commands
{
    public class UpdateCampCommandTest : TestBase
    {
        public UpdateCampCommandTest()
        {

        }

        [Fact]
        public async Task Handler_WhenUpdateCampWithoutMentorOrHead_ReturnSuccess()
        {
            //Arrange
            var userManager = GetUserManager();
            var unitOfWork = GetUnitOfWork();
            var mapper = _serviceProvider.GetRequiredService<IMapper>();
            var camp = new FakeCamp().Generate();
            await unitOfWork.Repository<Camp>().AddAsync(camp);
            await unitOfWork.SaveAsync();
            var validation = new UpdateCampCommandValidator();

            //Act
            var handler = new UpdateCampCommandHandler(unitOfWork, validation, userManager, mapper);
            var result = await handler.Handle(new()
            {
                id = camp.Id,
                DurationInWeeks = 2,
                Name = "hello",
                EndDate = DateOnly.MaxValue,
                startDate = DateOnly.MinValue,
                OpenForRegister = true,
                Term = Domain.Comman.Enums.Term.SecondTerm
            }, default);

            var dbCamp = await unitOfWork.Repository<Camp>().GetByIdAsync(camp.Id);

            //Assert
            result.IsSuccess.Should().BeTrue();
            dbCamp.Should().NotBeNull();
            dbCamp.EndDate.Should().Be(DateOnly.MaxValue);
            dbCamp.Name.Should().Be("hello");
            dbCamp.Term.Should().Be(Domain.Comman.Enums.Term.SecondTerm);
        }

        [Fact]
        public async Task Handler_WhenUpdateCampWithMentor_ReturnSuccess()
        {
            //Arrange
            var userManager = GetUserManager();
            var unitOfWork = GetUnitOfWork();
            var mapper = _serviceProvider.GetRequiredService<IMapper>();
            var validation = new UpdateCampCommandValidator();
            var camp = new FakeCamp().Generate();
            await unitOfWork.Repository<Camp>().AddAsync(camp);
            await unitOfWork.SaveAsync();
            var createMentor1 = await CreateUserAsync();

            var mentor1 = new Mentor()
            {
                Id = createMentor1.Id
            };
            await unitOfWork.Mentors.AddAsync(new() { Account = createMentor1, Member = mentor1 });
            await unitOfWork.Repository<MentorsOfCamp>().AddAsync(new() { CampId = camp.Id, MentorId = mentor1.Id });

            var createMentor2 = await CreateUserAsync();

            var mentor2 = new Mentor()
            {
                Id = createMentor2.Id
            };
            await unitOfWork.Mentors.AddAsync(new() { Account = createMentor2, Member = mentor2 });
            await unitOfWork.SaveAsync();
            //Act
            var handler = new UpdateCampCommandHandler(unitOfWork, validation, userManager, mapper);
            var result = await handler.Handle(new()
            {
                id = camp.Id,
                DurationInWeeks = 2,
                Name = "hello",
                EndDate = DateOnly.MaxValue,
                startDate = DateOnly.MinValue,
                OpenForRegister = true,
                Term = Domain.Comman.Enums.Term.SecondTerm,
                MentorsIds = [mentor2.Id]
            }, default);

            var mentorsOfCamp = await unitOfWork.Repository<MentorsOfCamp>().GetAllAsync();

            //Arrange
            result.IsSuccess.Should().BeTrue();
            mentorsOfCamp.Select(x => x.MentorId).Contains(mentor1.Id).Should().BeFalse();
            mentorsOfCamp.Select(x => x.MentorId).Contains(mentor2.Id).Should().BeTrue();
        }

        [Fact]
        public async Task Handler_WhenUpdateCampWithAddHead_ReturnSuccess()
        {
            //Arrange
            var userManager = GetUserManager();
            var unitOfWork = GetUnitOfWork();
            var mapper = _serviceProvider.GetRequiredService<IMapper>();
            var validation = new UpdateCampCommandValidator();
            var camp = new FakeCamp().Generate();
            await unitOfWork.Repository<Camp>().AddAsync(camp);
            await unitOfWork.SaveAsync();
            var headAccount = await CreateUserAsync();

            //Act
            var handler = new UpdateCampCommandHandler(unitOfWork, validation, userManager, mapper);
            var result = await handler.Handle(new()
            {
                id = camp.Id,
                DurationInWeeks = 2,
                Name = "hello",
                EndDate = DateOnly.MaxValue,
                startDate = DateOnly.MinValue,
                OpenForRegister = true,
                Term = Domain.Comman.Enums.Term.SecondTerm,
                HeadsIds = [headAccount.Id]
            }, default);
            var head = await unitOfWork.Heads.GetByIdAsync(headAccount.Id);
            var userInRoles = await userManager.GetRolesAsync(headAccount);

            //Assert
            result.IsSuccess.Should().BeTrue();
            head.Should().NotBeNull();
            head!.CampId.Should().Be(camp.Id);
            userInRoles.First().Should().Be(Roles.Head_Of_Camp);
        }

        [Fact]
        public async Task Handler_whenUpdateCampWithDeleteHead_ReturnHeadOfCampHasWithNoData()
        {
            //Arrange
            var userManager = GetUserManager();
            var unitOfWork = GetUnitOfWork();
            var mapper = _serviceProvider.GetRequiredService<IMapper>();
            var validation = new UpdateCampCommandValidator();
            var camp = new FakeCamp().Generate();
            await unitOfWork.Repository<Camp>().AddAsync(camp);
            await unitOfWork.SaveAsync();
            var headAccount = await CreateUserAsync();

            //Act
            var handler = new UpdateCampCommandHandler(unitOfWork, validation, userManager, mapper);
            var updateCamp = await handler.Handle(new()
            {
                id = camp.Id,
                DurationInWeeks = 2,
                Name = "hello",
                EndDate = DateOnly.MaxValue,
                startDate = DateOnly.MinValue,
                OpenForRegister = true,
                Term = Domain.Comman.Enums.Term.SecondTerm,
                HeadsIds = [headAccount.Id]
            }, default);

            var result = await handler.Handle(new()
            {
                id = camp.Id,
                DurationInWeeks = 2,
                Name = "hello",
                EndDate = DateOnly.MaxValue,
                startDate = DateOnly.MinValue,
                OpenForRegister = true,
                Term = Domain.Comman.Enums.Term.SecondTerm,
            }, default);

            var head = await unitOfWork.Heads.GetAllAsync();

            //Assert
            head.IsNullOrEmpty().Should().BeTrue();
        }

        [Fact]
        public async Task Handler_whenUpdateCampWithHeadAlreadyExist_ReturnHead()
        {
            //Arrange
            var userManager = GetUserManager();
            var unitOfWork = GetUnitOfWork();
            var mapper = _serviceProvider.GetRequiredService<IMapper>();
            var validation = new UpdateCampCommandValidator();
            var camp = new FakeCamp().Generate();
            await unitOfWork.Repository<Camp>().AddAsync(camp);
            await unitOfWork.SaveAsync();
            var headAccount = await CreateUserAsync();

            //Act
            var handler = new UpdateCampCommandHandler(unitOfWork, validation, userManager, mapper);
            for(int i = 0; i < 2; i++)
            {
                var updateCamp = await handler.Handle(new()
                {
                    id = camp.Id,
                    DurationInWeeks = 2,
                    Name = "hello",
                    EndDate = DateOnly.MaxValue,
                    startDate = DateOnly.MinValue,
                    OpenForRegister = true,
                    Term = Domain.Comman.Enums.Term.SecondTerm,
                    HeadsIds = [headAccount.Id]
                }, default);
            }

            var head = await unitOfWork.Heads.Entities.Where(x => x.CampId == camp.Id).ToListAsync();


            //Assert
            head.Count().Should().Be(1);
        }
    }
}
