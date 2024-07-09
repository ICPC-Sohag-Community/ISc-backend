using ISc.Application.Interfaces.Repos;
using ISc.Shared;
using MediatR;

namespace ISc.Application.Features.HeadOfCamps.Assigning.Commands.AssignTrainees
{
    public record AssignTraineeCommand : IRequest<Response>
    {
        public string TraineeId { get; set; }
        public string MentorId { get; set; }
    }

    internal class AssignTraineeCommandHandler : IRequestHandler<AssignTraineeCommand, Response>
    {
        private readonly IUnitOfWork _unitOfWork;

        public AssignTraineeCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Response> Handle(AssignTraineeCommand command, CancellationToken cancellationToken)
        {
            var trainee = await _unitOfWork.Trainees.GetByIdAsync(command.TraineeId);
            var mentor = await _unitOfWork.Mentors.GetByIdAsync(command.MentorId);

            if (trainee == null || mentor == null)
            {
                return await Response.FailureAsync("Invalid data.");
            }

            trainee.MentorId = mentor.Id;

            await _unitOfWork.Trainees.UpdateAsync(new() { Member = trainee });
            await _unitOfWork.SaveAsync();

            return await Response.SuccessAsync(trainee.Id, "Trainee assigned.");
        }
    }
}
