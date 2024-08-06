using ISc.Application.Interfaces.Repos;
using ISc.Domain.Models;
using ISc.Domain.Models.IdentityModels;
using ISc.Shared;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace ISc.Application.Features.Trainees.Feedbacks.Queries.GetAddFeedbackAccessablilty
{
    public record GetAddFeedbackAccessabiltyQuery : IRequest<Response>;

    internal class GetAddFeedbackAccessabiltyQueryHandler : IRequestHandler<GetAddFeedbackAccessabiltyQuery, Response>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly UserManager<Account> _userManager;

        public GetAddFeedbackAccessabiltyQueryHandler(
            IUnitOfWork unitOfWork,
            IHttpContextAccessor contextAccessor,
            UserManager<Account> userManager)
        {
            _unitOfWork = unitOfWork;
            _contextAccessor = contextAccessor;
            _userManager = userManager;
        }

        public async Task<Response> Handle(GetAddFeedbackAccessabiltyQuery request, CancellationToken cancellationToken)
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

            var LastSessionId = _unitOfWork.Repository<Session>().Entities
                               .Where(x => x.EndDate <= DateTime.UtcNow)
                               .OrderBy(x => x.StartDate)
                               .FirstOrDefaultAsync(cancellationToken)?.Id;

            if (LastSessionId is null)
            {
                return await Response.FailureAsync("No session found.");
            }

            var hasNoFeedback = !await _unitOfWork.Repository<SessionFeedback>().Entities
                               .AnyAsync(x => x.TraineeId == trainee.Id && LastSessionId == x.SessionId);

            return await Response.SuccessAsync(hasNoFeedback);
        }
    }
}
