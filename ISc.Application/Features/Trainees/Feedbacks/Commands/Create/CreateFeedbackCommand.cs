using ISc.Application.Interfaces.Repos;
using ISc.Domain.Models;
using ISc.Domain.Models.IdentityModels;
using ISc.Shared;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ISc.Application.Features.Trainees.Feedbacks.Commands.Create
{
    public record CreateFeedbackCommand:IRequest<Response>
    {
        public int SessionId { get; set; }
        public int Rate { get; set; }
        public string Feedback { get; set; }
    }

    internal class CreateFeedbackCommandHandler : IRequestHandler<CreateFeedbackCommand, Response>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly UserManager<Account> _userManager;

        public CreateFeedbackCommandHandler(
            IUnitOfWork unitOfWork,
            IHttpContextAccessor contextAccessor,
            UserManager<Account> userManager)
        {
            _unitOfWork = unitOfWork;
            _contextAccessor = contextAccessor;
            _userManager = userManager;
        }

        public async Task<Response> Handle(CreateFeedbackCommand command, CancellationToken cancellationToken)
        {
            var user = await _userManager.GetUserAsync(_contextAccessor.HttpContext!.User);

            if (user is null)
            {
                return await Response.FailureAsync("Unauthorized.", HttpStatusCode.Unauthorized);
            }

            var trainee = await _unitOfWork.Trainees.GetByIdAsync(user.Id);

            if (trainee == null)
            {
                return await Response.FailureAsync("Unauthorized.", HttpStatusCode.Unauthorized);
            }

            var feedback = command.Adapt<SessionFeedback>();
            feedback.TraineeId = trainee.Id;

            await _unitOfWork.Repository<SessionFeedback>().AddAsync(feedback);
            await _unitOfWork.SaveAsync();

            return await Response.SuccessAsync(feedback);
        }
    }
}
