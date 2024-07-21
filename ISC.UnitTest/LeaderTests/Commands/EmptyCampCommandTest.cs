using FluentAssertions;
using ISc.Application.Features.Leader.Camps.Commands.Empty;
using ISc.Domain.Models;
using ISc.Domain.Models.CommunityStaff;
using ISc.UnitTests.FakesObjects;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISc.UnitTests.LeaderTests.Commands
{
    public class EmptyCampCommandTest:TestBase
    {
        public EmptyCampCommandTest()
        {
            
        }

        [Fact]
        public async Task Handler_WhenEmptyCampHasMentorAndTraineeAndHead_ReturnSuccess()
        {
            //Arrange
            var unitOfWork = GetUnitOfWork();
            var camp = new FakeCamp().Generate();
            await unitOfWork.Repository<Camp>().AddAsync(camp);
            await unitOfWork.SaveAsync();

            var createTrainee = await CreateUserAsync();
            var traniee = new Trainee()
            {
                Id = createTrainee.Id,
                CampId = camp.Id
            };
            var createHead = await CreateUserAsync();
            var head = new HeadOfCamp()
            {
                Id = createHead.Id,
                CampId = camp.Id
            };

            var createMentor = await CreateUserAsync();
            var mentor = new Mentor()
            {
                Id = createMentor.Id
            };
            await unitOfWork.Trainees.AddAsync(new() { Account = createTrainee, Member = traniee });
            await unitOfWork.Mentors.AddAsync(new() { Account = createMentor, Member = mentor });
            await unitOfWork.Heads.AddAsync(new() { Account = createHead, Member = head });
            await unitOfWork.SaveAsync();
            await unitOfWork.Repository<MentorsOfCamp>().AddAsync(new() { CampId = camp.Id, MentorId = mentor.Id });
            await unitOfWork.SaveAsync();

            var handler = new EmptyCampCommandHandler(unitOfWork);

            //Act
            var result = await handler.Handle(new(camp.Id), default);
            var camps = await unitOfWork.Repository<Camp>().GetAllAsync();
            var trainees = await unitOfWork.Trainees.GetAllAsync();
            var mentors = await unitOfWork.Repository<MentorsOfCamp>().GetAllAsync();
            var heads = await unitOfWork.Heads.GetAllAsync();

            //Assert
            result.IsSuccess.Should().BeTrue();
            camps.Count().Should().Be(1);
            trainees.IsNullOrEmpty().Should().BeTrue();
            mentors.Count().Should().Be(0);
            heads.Count().Should().Be(1);          
        }

        [Fact]
        public async Task Handler_WhenCampIdIsInCorrect_ReturnFailResponse()
        {
            //Arrange
            var unitOfWork = GetUnitOfWork();

            var handler = new EmptyCampCommandHandler(unitOfWork);

            //Act
            var result = await handler.Handle(new(1), default);

            //Assert
            result.IsSuccess.Should().BeFalse();
        }
    }
}
