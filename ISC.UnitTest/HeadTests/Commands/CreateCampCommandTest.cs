using Bogus;
using FluentAssertions;
using ISc.Application.Features.Leader.Camps.Commands.Create;
using ISc.Domain.Comman.Constant;
using ISc.Domain.Comman.Dtos;
using ISc.Domain.Comman.Enums;
using ISc.Domain.Models;
using ISc.Domain.Models.CommunityStaff;
using ISc.Domain.Models.IdentityModels;
using ISc.UnitTests.FakesOjbects;
using Microsoft.EntityFrameworkCore;

namespace ISc.UnitTests.HeadTest.Commands
{
    public class CreateCampCommandTest : TestBase
    {
        public CreateCampCommandTest()
        {

        }

        #region Success Path
        [Fact]
        public async Task Handler_WhenCreate_Success()
        {
            //Arrange
            var unitOfWork = GetUnitOfWork();
            var userManager = GetUserManager();

            var validator = new CreateCampCommandValidator();
            var handler = new CreateCampCommandHandler(unitOfWork, validator, userManager);

            //Act
            var result = await handler.Handle(new()
            {
                Name = "NewComer",
                startDate = DateOnly.MinValue,
                EndDate = DateOnly.MaxValue,
                DurationInWeeks = 5,
                OpenForRegister = true,
                Term = Term.FirstTerm
            }, default);

            //Assert
            result.Data.Should().Be(1);
        }

        [Fact]
        public async Task Handler_WhenCreateWithMentorsInAnotherCamp_Success()
        {
            //Arrange
            var unitOfWork = GetUnitOfWork();
            var userManager = GetUserManager();

            var validator = new CreateCampCommandValidator();
            var handler = new CreateCampCommandHandler(unitOfWork, validator, userManager);
            var Account = await CreateUserAsync();
            var mentor = new Mentor()
            {
                Id = Account.Id,
                About = "hello",
            };

            await unitOfWork.Mentors.AddAsync(new()
            {
                Account = Account,
                Member = mentor,
            });

            var camp1 = await handler.Handle(new()
            {
                Name = "NewComer",
                startDate = DateOnly.MinValue,
                EndDate = DateOnly.MaxValue,
                DurationInWeeks = 5,
                OpenForRegister = true,
                Term = Term.FirstTerm,
                MentorsIds = [mentor.Id]
            }, default);

            //Act
            var result = await handler.Handle(new()
            {
                Name = "phase1",
                startDate = DateOnly.MinValue,
                EndDate = DateOnly.MaxValue,
                DurationInWeeks = 5,
                OpenForRegister = true,
                Term = Term.FirstTerm,
                MentorsIds = [mentor.Id]
            }, default);


            //Assert
            result.IsSuccess.Should().BeTrue();

            var assignedtrainee = await unitOfWork.Repository<MentorsOfCamp>().Entities
                .SingleOrDefaultAsync(x=>x.CampId==(int)result.Data!&&x.MentorId==mentor.Id);

            assignedtrainee.Should().NotBeNull();
        }

        [Fact]
        public async Task Handler_WhenCreateWithMentorsNotInCamp_Success()
        {

        }

        [Fact]
        public async Task Handler_WhenCreateWithHeadsNotExist_Success()
        {
            //Arrange
            var unitOfWork = GetUnitOfWork();
            var userManager = GetUserManager();

            var validator = new CreateCampCommandValidator();
            var handler = new CreateCampCommandHandler(unitOfWork, validator, userManager);
            var Account = await CreateUserAsync();
            var mentor = new Mentor()
            {
                Id = Account.Id,
                About = "hello",
            };

            await unitOfWork.Mentors.AddAsync(new()
            {
                Account = Account,
                Member = mentor,
            });

            //Act
            var result = await handler.Handle(new()
            {
                Name = "NewComer",
                startDate = DateOnly.MinValue,
                EndDate = DateOnly.MaxValue,
                DurationInWeeks = 5,
                OpenForRegister = true,
                Term = Term.FirstTerm,
                HeadsIds = new List<string>() { mentor.Id }
            }, default);

            //Assert
            result.Data.Should().Be(1);
            var heads = await unitOfWork.Heads.GetAllAsync();
            heads.Should().NotBeNull();
            heads!.Count().Should().Be(1);
            heads!.First().CampId.Should().Be(1);
        }

        [Fact]
        public async Task Handler_WheCreateWithHeadsExist_Success()
        {
            //Arrange
            var unitOfWork = GetUnitOfWork();
            var userManager = GetUserManager();

            var validator = new CreateCampCommandValidator();
            var handler = new CreateCampCommandHandler(unitOfWork, validator, userManager);
            var camp1 = await handler.Handle(new()
            {
                Name = "NewComer",
                startDate = DateOnly.MinValue,
                EndDate = DateOnly.MaxValue,
                DurationInWeeks = 5,
                OpenForRegister = true,
                Term = Term.FirstTerm
            }, default);
            var account = await CreateUserAsync();
            var head = new HeadOfCamp()
            {
                CampId = (int)camp1.Data!,
                Id = account.Id
            };

            await unitOfWork.Heads.AddAsync(new()
            {
                Member = head,
                Account = account,
            });

            //Act
            var result = await handler.Handle(new()
            {
                Name = "phase1",
                startDate = DateOnly.MinValue,
                EndDate = DateOnly.MaxValue,
                DurationInWeeks = 5,
                OpenForRegister = true,
                Term = Term.FirstTerm,
                HeadsIds = [head.Id]
            }, default);

            var assignedHead = await unitOfWork.Heads.GetByIdAsync(head.Id);

            //Assert
            result.IsSuccess.Should().BeTrue();
            assignedHead.Should().NotBeNull();
            assignedHead!.CampId.Should().Be((int)result.Data!);
        }
        #endregion

        #region Failure path
        [Fact]
        public async Task Handler_WhenCampAlreadyFoundWithSameName_ReturnIsSuccessEqualFalse()
        {
            //Arrange
            var unitOfWork = GetUnitOfWork();
            var userManager = GetUserManager();

            var validator = new CreateCampCommandValidator();
            var handler = new CreateCampCommandHandler(unitOfWork, validator, userManager);

            //Act
            var camp = await handler.Handle(new()
            {
                Name = "NewComer",
                startDate = DateOnly.MinValue,
                EndDate = DateOnly.MaxValue,
                DurationInWeeks = 5,
                OpenForRegister = true,
                Term = Term.FirstTerm
            }, default);

            var result = await handler.Handle(new()
            {
                Name = "NewComer",
                startDate = DateOnly.MinValue,
                EndDate = DateOnly.MaxValue,
                DurationInWeeks = 5,
                OpenForRegister = true,
                Term = Term.FirstTerm
            }, default);

            //Assert
            result.IsSuccess.Should().BeFalse();
        }

        [Fact]
        public async Task Handler_WhenValidationFailBySendEndDateWitNull_ReturnIsSuccessEqualFalseAndErrorContainData()
        {
            //Arrange
            var unitOfWork = GetUnitOfWork();
            var userManager = GetUserManager();

            var validator = new CreateCampCommandValidator();
            var handler = new CreateCampCommandHandler(unitOfWork, validator, userManager);

            //Act
            var result = await handler.Handle(new()
            {
                Name = "NewComer",
                startDate = DateOnly.MinValue,
                DurationInWeeks = 5,
                OpenForRegister = false,
                Term = Term.FirstTerm
            }, default);

            //Assert
            result.Errors.Count.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task Handler_WhenMentorIdsIsNotNullAndIdNotContainInMentorsTable_ReturnIsSuccessFalse()
        {
            //Arrange
            var unitOfWork = GetUnitOfWork();
            var userManager = GetUserManager();
            var roleManager = GetRoleManager();

            var roles = await roleManager.Roles.ToListAsync();
            var user = await CreateUserAsync();

            await userManager.AddToRoleAsync(user, Roles.Mentor);

            var mentor = new Mentor()
            {
                Id = user.Id,
                About = "about",
                Account = user
            };

            await unitOfWork.Mentors.AddAsync(new AccountModel<Mentor>()
            {
                Account = user,
                Member = mentor
            });

            var validator = new CreateCampCommandValidator();
            var handler = new CreateCampCommandHandler(unitOfWork, validator, userManager);

            //Act
            var result = await handler.Handle(new()
            {
                Name = "NewComer",
                startDate = DateOnly.MinValue,
                EndDate = DateOnly.MaxValue,
                DurationInWeeks = 5,
                OpenForRegister = true,
                Term = Term.FirstTerm,
                MentorsIds = [mentor.Id]
            }, default);

            //Assert
            result.IsSuccess.Should().BeFalse();
        }
        #endregion
    }
}
