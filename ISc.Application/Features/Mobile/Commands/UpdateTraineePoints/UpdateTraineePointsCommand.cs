using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISc.Application.Interfaces.Repos;
using ISc.Shared;
using MediatR;

namespace ISc.Application.Features.Mobile.Commands.UpdateTraineePoints
{
    public record UpdateTraineePointsCommand:IRequest<Response>
    {
        public string TraineeId { get; set; }
        public int Point { get; set; }
    }

    internal class UpdateTraineePointsQueryHandler : IRequestHandler<UpdateTraineePointsCommand, Response>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateTraineePointsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Response> Handle(UpdateTraineePointsCommand query, CancellationToken cancellationToken)
        {
            var trainee = await _unitOfWork.Trainees.GetByIdAsync(query.TraineeId);

            if (trainee == null)
            {
                return await Response.FailureAsync("Trainee not found.");
            }

            trainee.Points += query.Point;

            await _unitOfWork.Trainees.UpdateAsync(new()
            {
                Account = trainee.Account,
                Member = trainee
            });
            await _unitOfWork.SaveAsync();

            return await Response.SuccessAsync("Success");
        }
    }
}
