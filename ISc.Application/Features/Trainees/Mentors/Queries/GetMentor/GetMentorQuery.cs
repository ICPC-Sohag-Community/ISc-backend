using ISc.Application.Interfaces;
using ISc.Application.Interfaces.Repos;
using ISc.Domain.Models.IdentityModels;
using ISc.Shared;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Net;

namespace ISc.Application.Features.Trainees.Mentors.Queries.GetMentor
{
    public record GetMentorQuery : IRequest<Response>;

    internal class GetMentorQueryHandler : IRequestHandler<GetMentorQuery, Response>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly UserManager<Account> _userManager;
        private readonly IMediaServices _mediaServices;

        public GetMentorQueryHandler(
            IUnitOfWork unitOfWork,
            IHttpContextAccessor contextAccessor,
            UserManager<Account> userManager,
            IMediaServices mediaServices)
        {
            _unitOfWork = unitOfWork;
            _contextAccessor = contextAccessor;
            _userManager = userManager;
            _mediaServices = mediaServices;
        }

        public async Task<Response> Handle(GetMentorQuery request, CancellationToken cancellationToken)
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

            var mentor = trainee.Mentor?.Account;

            if (mentor is null)
            {
                return await Response.FailureAsync("No assigned mentor.");
            }

            mentor.PhotoUrl = _mediaServices.GetUrl(mentor.PhotoUrl);

            return await Response.SuccessAsync(mentor);
        }
    }
}
