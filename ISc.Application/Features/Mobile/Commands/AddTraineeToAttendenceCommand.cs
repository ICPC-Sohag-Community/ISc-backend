using ISc.Application.Interfaces.Repos;
using ISc.Domain.Models;
using ISc.Shared;
using MediatR;

namespace ISc.Application.Features.Mobile.Commands
{
    public class AddTraineeToAttendenceCommand:IRequest<Response>
    {
        public string TraineeId { get; set; }


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
            var trainee = _unitOfWork.Trainees.Entities.FirstOrDefault(i => i.Id == command.TraineeId);

            if (trainee==null)
                return await Response.FailureAsync("The Required Trainee Is Not Exist");
            
            var SessionId=trainee?.Camp?.Sessions.FirstOrDefault(i=>i.StartDate>=DateTime.Now.Date)?.Id;

            if (SessionId == null) 
                return await Response.FailureAsync("The Required Trainee Is Not Exist");

            await _unitOfWork.Repository<TraineeAttendence>().AddAsync(new TraineeAttendence { SessionId = SessionId??0, TraineeId = command.TraineeId });
            return await Response.SuccessAsync("Success");
        }
    }
}
