using ISc.Application.Interfaces.Repos;
using ISc.Shared;
using Mapster;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISc.Application.Features.Mentors.TraineesInfo.Queries.GetTraineeInDetail
{
    public record GetTraineeInDetailQuery : IRequest<Response>
    {
        public string TraineeId { get; set; }

        public GetTraineeInDetailQuery(string traineeId)
        {
            TraineeId = traineeId;
        }
    }

    internal class GetTraineeInDetailQueryHandler : IRequestHandler<GetTraineeInDetailQuery, Response>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetTraineeInDetailQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Response> Handle(GetTraineeInDetailQuery query, CancellationToken cancellationToken)
        {
            var entity = await _unitOfWork.Trainees.GetByIdAsync(query.TraineeId);

            if (entity is null)
            {
                return await Response.FailureAsync("Trainee not found.", System.Net.HttpStatusCode.NotFound);
            }

            var trainee = entity.Account.Adapt<GetTraineeInDetailQueryDto>();

            return await Response.SuccessAsync(trainee);
        }
    }
}
