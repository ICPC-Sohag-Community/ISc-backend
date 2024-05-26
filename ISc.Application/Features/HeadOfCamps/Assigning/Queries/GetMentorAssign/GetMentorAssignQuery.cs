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

        public GetMentorAssignQueryHandler(
            UserManager<Account> userManager,
            IUnitOfWork unitOfWork,
            IHttpContextAccessor context)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _context = context;
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
                return await Response.FailureAsync("not allowed", System.Net.HttpStatusCode.MethodNotAllowed);
            }

            var mentors = _unitOfWork.Repository<Camp>().Entities.SingleOrDefaultAsync(x => x.Id == head.CampId).Result?.Mentors
                            .Adapt<List<GetMentorAssignQueryDto>>();

            if (mentors == null)
            {
                return await Response.SuccessAsync();
            }

            foreach (var mentor in mentors)
            {
                mentor.Trainees = await _unitOfWork.Trainees.Entities
                    .Where(x => x.MentorId == mentor.Id)
                    .ProjectToType<GetTraineeForMentorAssign>()
                    .ToListAsync();
            }

            return await Response.SuccessAsync(mentors);
        }
    }
}
