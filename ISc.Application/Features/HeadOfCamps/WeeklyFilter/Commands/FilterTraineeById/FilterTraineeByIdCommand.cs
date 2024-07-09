using ISc.Application.Interfaces;
using ISc.Application.Interfaces.Repos;
using ISc.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ISc.Application.Features.HeadOfCamps.WeeklyFilter.Commands.FilterTraineeById
{
    public record FilterTraineeByIdCommand:IRequest<Response>
    {
        public string Id { get; set; }

        public FilterTraineeByIdCommand(string id)
        {
            Id = id;
        }
    }

    internal class FilterTraineeByIdCommnadHandler : IRequestHandler<FilterTraineeByIdCommand, Response>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailSender _emailSender;

        public FilterTraineeByIdCommnadHandler(
            IUnitOfWork unitOfWork,
            IEmailSender emailSender)
        {
            _unitOfWork = unitOfWork;
            _emailSender = emailSender;
        }

        public async Task<Response> Handle(FilterTraineeByIdCommand command, CancellationToken cancellationToken)
        {
            var trainee = await _unitOfWork.Trainees.GetByIdAsync(command.Id);

            if(trainee is null)
            {
                return await Response.FailureAsync("trainee not found.",HttpStatusCode.NotFound);
            }

            var account = trainee.Account;
            var mentorEmail = trainee.Mentor?.Account.Email ?? "";
            await _emailSender.SendKickedoutEmailAsync(account.Email!, trainee.Camp.Name, account.FirstName + ' ' + account.MiddleName,mentorEmail);

            await _unitOfWork.Trainees.DeleteAsync(trainee.Account, trainee, false);
            await _unitOfWork.SaveAsync();

            return await Response.SuccessAsync("Trainee filtered from camp.");
        }
    }
}
