using FluentAssertions;
using ISc.Application.Features.Leader.Camps.Commands.Delete;
using ISc.Domain.Models;
using ISc.Domain.Models.CommunityStaff;
using ISc.UnitTests.FakesOjbects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISc.UnitTests.LeaderTests.Commands
{
    
    public class DeleteCampCommandTest:TestBase
    {
        public DeleteCampCommandTest()
        {
            
        }

        [Fact]
        public async Task Handler_WhenDeleteCampWithNoDependinces_Success()
        {
            var unitOfWork = GetUnitOfWork();
            var camp = new FakeCamp().Generate();
            await unitOfWork.Repository<Camp>().AddAsync(camp);
            await unitOfWork.SaveAsync();
            var handler = new DeleteCampCommandHandler(unitOfWork);

            var result = await handler.Handle(new DeleteCampCommand(camp.Id), default);
            var entity= await unitOfWork.Repository<Camp>().GetByIdAsync(camp.Id);

            result.IsSuccess.Should().BeTrue();
            entity.Should().BeNull();
        }

        [Fact]
        public async Task Handler_WhenDeleteCampWithMembers_Success()
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
            var createHead= await CreateUserAsync();
            var head = new HeadOfCamp()
            {
                Id = createHead.Id,
                CampId = camp.Id
            };

            var createMentor=await CreateUserAsync();
            var mentor = new Mentor()
            {
                Id = createMentor.Id
            };
            await unitOfWork.Trainees.AddAsync(new() { Account = createTrainee, Member = traniee });
            await unitOfWork.Mentors.AddAsync(new() { Account = createMentor, Member = mentor });
            await unitOfWork.Heads.AddAsync(new() { Account = createHead, Member = head });
            await unitOfWork.SaveAsync();
            await unitOfWork.Repository<MentorsOfCamp>().AddAsync(new() { CampId = camp.Id,MentorId=mentor.Id });
            await unitOfWork.SaveAsync();


            //Act
            var handler = new DeleteCampCommandHandler(unitOfWork);
            var result = await handler.Handle(new DeleteCampCommand(camp.Id), default);
            var entity = await unitOfWork.Repository<Camp>().GetByIdAsync(camp.Id);
            var campMentor = await unitOfWork.Repository<MentorsOfCamp>().Entities.SingleOrDefaultAsync(x => x.MentorId == mentor.Id);
            var campHead = await unitOfWork.Heads.Entities.SingleOrDefaultAsync(x => x.CampId == camp.Id);
            var campTrainee = await unitOfWork.Trainees.Entities.SingleOrDefaultAsync(x => x.CampId == camp.Id);

            //Assert
            result.IsSuccess.Should().BeTrue();
            entity.Should().BeNull();
            campMentor.Should().BeNull();
            campHead.Should().BeNull();
            campTrainee.Should().BeNull();
        }
    }
}
