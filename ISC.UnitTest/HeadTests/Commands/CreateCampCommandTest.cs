using FluentAssertions;
using ISc.Application.Features.Leader.Camps.Commands.Create;
using ISc.Domain.Comman.Constant;
using ISc.Domain.Comman.Dtos;
using ISc.Domain.Comman.Enums;
using ISc.Domain.Models.CommunityStaff;
using Microsoft.AspNetCore.Identity;
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

        public async Task Handler_WhenCreateWithMentors_Success()
        {

        }

        public async Task Handler_WhenCreateWithHeads_Success()
        {

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
            var user = await CreateUser();

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
