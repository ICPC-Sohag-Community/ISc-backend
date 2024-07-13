using ISc.Application.Interfaces.Repos;
using ISc.Domain.Models;
using ISc.Shared;
using MediatR;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;

namespace ISc.Application.Features.Mobile.Commands.AddAttendnce
{
    public class AddTraineeToAttendenceCommand : IRequest<Response>
    {
        public string TraineeId { get; set; }
        public int CampId { get; set; }
    }
    internal class AddTraineeToAttendenceCommandHandler : IRequestHandler<AddTraineeToAttendenceCommand, Response>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDataProtector _dataProtector;

        public AddTraineeToAttendenceCommandHandler(IUnitOfWork unitOfWork, IDataProtectionProvider dataProvider)
        {
            _unitOfWork = unitOfWork;
            _dataProtector = dataProvider.CreateProtector("SecureId");
        }

        public async Task<Response> Handle(AddTraineeToAttendenceCommand command, CancellationToken cancellationToken)
        {
            //TODO: command.TraineeId = _dataProtector.Unprotect(command.TraineeId);

            var trainee = _unitOfWork.Trainees.Entities.FirstOrDefault(i => i.Id == command.TraineeId);

            if (trainee == null)
            {
                return await Response.FailureAsync("Trainee not found.");
            }

            if (trainee.CampId != command.CampId)
            {
                return await Response.FailureAsync("No Current seession for this trainee.");
            }

            var Session = trainee.Camp.Sessions.FirstOrDefault(i => i.StartDate.Date >= DateTime.Now.Date);

            if (Session == null)
            {
                return await Response.FailureAsync("There is no session for now.");
            }

            if(await _unitOfWork.Repository<TraineeAttendence>().Entities.AnyAsync(x=>x.TraineeId == command.TraineeId && x.SessionId == Session.Id))
            {
                return await Response.FailureAsync("Trainee already attend.");
            }

            await _unitOfWork.Repository<TraineeAttendence>()
                            .AddAsync(new TraineeAttendence
                            {
                                SessionId = Session.Id,
                                TraineeId = command.TraineeId
                            });

            await _unitOfWork.SaveAsync();

            return await Response.SuccessAsync("Success");
        }
    }
}
