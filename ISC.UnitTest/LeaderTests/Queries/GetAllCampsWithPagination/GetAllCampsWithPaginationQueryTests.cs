using Bogus;
using FluentAssertions;
using ISc.Application.Features.Leader.Camps.Queries.GetAllCampsWithPagination;
using ISc.Domain.Models;
using ISc.UnitTests.FakesObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISc.UnitTests.LeaderTests.Queries.GetAllCampsWithPagination
{
    public class GetAllCampsWithPaginationQueryTests : TestBase
    {
        [Fact]
        public async Task Handel_WhenCampsExcises_ReturnPaginateResponse()
        {
            // Arrange
            var unitOfWork = GetUnitOfWork();
            var fakeCamp = new FakeCamp();

            var fakeCamps = fakeCamp.Generate(10);
            await unitOfWork.Repository<Camp>().AddRangeAsync(fakeCamps);
            await unitOfWork.SaveAsync(); 

            var query = new GetAllCampsWithPaginationQuery
            {
                PageNumber = 1,
                PageSize = 15
            };
            var handler = new GetAllCampsWithPaginationQueryHandler(unitOfWork);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Data.Should().NotBeNull();

            var data = result.Data as List<GetAllCampsWithPaginationQueryDto>;
            data.Should().NotBeNull();
            data.First().Name.Should().Be(fakeCamps.First().Name);
        }
    }
}