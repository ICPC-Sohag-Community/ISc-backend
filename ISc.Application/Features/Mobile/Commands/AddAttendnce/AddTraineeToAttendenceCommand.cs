﻿using ISc.Application.Interfaces.Repos;
using ISc.Domain.Models;
using ISc.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ISc.Application.Features.Mobile.Commands.AddAttendnce
{
    public class AddTraineeToAttendenceCommand : IRequest<Response>
    {
        public string TraineeId { get; set; }

        public AddTraineeToAttendenceCommand(string traineeId)
        {
            TraineeId = traineeId;
        }
    }
    internal class AddTraineeToAttendenceCommandHandler : IRequestHandler<AddTraineeToAttendenceCommand, Response>
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddTraineeToAttendenceCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Response> Handle(AddTraineeToAttendenceCommand command, CancellationToken cancellationToken)
        {
            var trainee = _unitOfWork.Trainees.Entities.FirstOrDefault(i => i.Id == command.TraineeId);

            if (trainee == null)
            {
                return await Response.FailureAsync("Trainee not found.");
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
