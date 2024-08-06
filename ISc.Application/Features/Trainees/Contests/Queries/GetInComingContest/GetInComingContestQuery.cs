using ISc.Application.Interfaces.Repos;
using ISc.Domain.Models;
using ISc.Domain.Models.IdentityModels;
using ISc.Shared;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace ISc.Application.Features.Trainees.Contests.Queries.GetInComingContest
{
    public record GetInComingContestQuery : IRequest<Response>;

    internal class GetInComingContestQueryHandler : IRequestHandler<GetInComingContestQuery, Response>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly UserManager<Account> _userManager;

        public GetInComingContestQueryHandler(
            IUnitOfWork unitOfWork,
            IHttpContextAccessor contextAccessor,
            UserManager<Account> userManager)
        {
            _unitOfWork = unitOfWork;
            _contextAccessor = contextAccessor;
            _userManager = userManager;
        }

        public async Task<Response> Handle(GetInComingContestQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.GetUserAsync(_contextAccessor.HttpContext!.User);

            if (user is null)
            {
                return await Response.FailureAsync("Unauthorized.", HttpStatusCode.Unauthorized);
            }

            var trainee = await _unitOfWork.Trainees.GetByIdAsync(user.Id);

            if (trainee is null)
            {
                return await Response.FailureAsync("Unauthorized.", HttpStatusCode.Unauthorized);
            }

            var entity = await _unitOfWork.Repository<Contest>().Entities
                        .Where(x => x.CampId == trainee.CampId && (x.StartTime >= DateTime.UtcNow || x.EndTime > DateTime.UtcNow))
                        .OrderBy(x => x.StartTime)
                        .FirstOrDefaultAsync(cancellationToken);

            var contest = entity.Adapt<GetInComingContestQueryDto>();

            if (entity is not null)
            {
                contest.IsRunning = DateTime.UtcNow >= entity.StartTime;
                var remainTime = contest.IsRunning ? entity.EndTime - entity.StartTime : entity.StartTime - DateTime.UtcNow;
                contest.RemainTime = new()
                {
                    Days = remainTime.Days,
                    Hours = remainTime.Hours,
                    Minutes = remainTime.Minutes,
                    Seconds = remainTime.Seconds
                };
            }

            return await Response.SuccessAsync(contest);
        }
    }
}
