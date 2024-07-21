using ISc.Application.Interfaces;
using ISc.Application.Interfaces.Repos;
using ISc.Domain.Models;
using ISc.Domain.Models.IdentityModels;
using ISc.Shared;
using Mapster;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ISc.Application.Features.HeadOfCamps.Assigning.Queries.GetMentorAssign
{
    public record GetMentorAssignQuery : IRequest<Response>;

    internal class GetMentorAssignQueryHandler : IRequestHandler<GetMentorAssignQuery, Response>
    {
        private readonly UserManager<Account> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _context;
        private readonly IMediaServices _mediaServices;

        public GetMentorAssignQueryHandler(
            UserManager<Account> userManager,
            IUnitOfWork unitOfWork,
            IHttpContextAccessor context,
            IMediaServices mediaServices)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _context = context;
            _mediaServices = mediaServices;
        }

        public async Task<Response> Handle(GetMentorAssignQuery query, CancellationToken cancellationToken)
        {
            var user = await _userManager.GetUserAsync(_context.HttpContext!.User);

            if (user is null)
            {
                return await Response.FailureAsync("Unauthorized", System.Net.HttpStatusCode.Unauthorized);
            }

            var head = await _unitOfWork.Heads.GetByIdAsync(user.Id);

            if (head is null)
            {
                return await Response.FailureAsync("Not allowed", System.Net.HttpStatusCode.MethodNotAllowed);
            }

            var mentors = await _unitOfWork.Repository<MentorsOfCamp>().Entities
                            .Where(x => x.CampId == head.CampId).Select(x => x.Mentor.Account)
                            .ProjectToType<GetMentorAssignQueryDto>()
                            .ToListAsync();

            if (mentors == null)
            {
                return await Response.SuccessAsync();
            }

            foreach (var mentor in mentors)
            {
                mentor.Trainees = await _unitOfWork.Trainees.Entities
                    .Where(x => x.MentorId == mentor.Id)
                    .ProjectToType<GetTraineeForMentorAssignDto>()
                    .ToListAsync();

                foreach(var trainee in mentor.Trainees)
                {
                    trainee.PhotoUrl = _mediaServices.GetUrl(trainee.PhotoUrl);
                }
            }

            return await Response.SuccessAsync(mentors);
        }
    }
}
