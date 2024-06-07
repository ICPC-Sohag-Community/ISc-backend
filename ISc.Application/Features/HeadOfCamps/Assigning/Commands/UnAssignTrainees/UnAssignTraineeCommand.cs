using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISc.Application.Interfaces.Repos;
using ISc.Shared;
using MediatR;

namespace ISc.Application.Features.HeadOfCamps.Assigning.Commands.UnAssignTrainees
{
    public record UnAssignTraineeCommand : IRequest<Response>
    {
        public string TraineeId { get; set; }

        public UnAssignTraineeCommand(string traineeId)
        {
            TraineeId = traineeId;
        }
    }

    internal class UnAssignTraineeCommandHandler : IRequestHandler<UnAssignTraineeCommand, Response>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UnAssignTraineeCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Response> Handle(UnAssignTraineeCommand command, CancellationToken cancellationToken)
        {
            var trainee = await _unitOfWork.Trainees.GetByIdAsync(command.TraineeId);

            if (trainee == null)
            {
                return await Response.FailureAsync("Failled to find one of users.",System.Net.HttpStatusCode.NotFound);
            }

            trainee.MentorId = null;

            await _unitOfWork.Trainees.UpdateAsync(new() { Member = trainee });
            await _unitOfWork.SaveAsync();

            return await Response.SuccessAsync(trainee.Id, "unassigned to mentor");
        }
    }
}
