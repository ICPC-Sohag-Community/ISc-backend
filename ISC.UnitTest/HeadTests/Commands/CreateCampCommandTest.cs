using FluentAssertions;
using ISc.Application.Features.Leader.Camps.Commands.Create;
using ISc.Domain.Comman.Enums;

namespace ISC.UnitTests.HeadTest.Commands
{
    public class CreateCampCommandTest : TestBase
    {
        public CreateCampCommandTest()
        {

        }

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
    }
}
