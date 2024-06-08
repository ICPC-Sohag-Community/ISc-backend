using ISc.Application.Interfaces.Repos;
using ISc.Domain.Models;
using ISc.Shared;
using MediatR;

namespace ISc.Application.Features.Mobile.Commands
{
    public class AddTraineeToAttendenceCommand:IRequest<Response>
    {
        public string TraineeId { get; set; }

        public int SessionId { get; set; }

    }
    internal class AddTraineeToAttendenceCommandHandler : IRequestHandler<AddTraineeToAttendenceCommand, Response>
    {
        private readonly  IUnitOfWork _unitOfWork;

        public AddTraineeToAttendenceCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Response> Handle(AddTraineeToAttendenceCommand command, CancellationToken cancellationToken)
        {
            if (!_unitOfWork.Trainees.Entities.Any(i => i.Id == command.TraineeId))
                return await Response.FailureAsync("The Required Trainee Is Not Exist");
            
            if (!_unitOfWork.Repository<Session>().Entities.Any(i => i.Id == command.SessionId))
                return await Response.FailureAsync("The Required Session Is Not Exist");

            await _unitOfWork.Repository<TraineeAttendence>().AddAsync(new TraineeAttendence { SessionId = command.SessionId, TraineeId = command.TraineeId });
            return await Response.SuccessAsync("Success");
        }
    }
}
