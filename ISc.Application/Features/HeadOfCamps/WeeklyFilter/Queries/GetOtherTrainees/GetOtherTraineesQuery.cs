using ISc.Application.Features.HeadOfCamps.WeeklyFilter.Queries.GetToFilter;
using ISc.Application.Interfaces.Repos;
using ISc.Domain.Comman.Enums;
using ISc.Domain.Models;
using ISc.Domain.Models.CommunityStaff;
using ISc.Domain.Models.IdentityModels;
using ISc.Shared;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;

namespace ISc.Application.Features.HeadOfCamps.WeeklyFilter.Queries.GetOtherTrainees
{
    public class GetOtherTraineesQuery : IRequest<Response>
    {
        public List<string> ToFilterIds { get; set; }

        public GetOtherTraineesQuery(List<string> toFilterIds)
        {
            ToFilterIds = toFilterIds;
        }
    }

    internal class GetOtherTraineesQueryHandler : IRequestHandler<GetOtherTraineesQuery, Response>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<Account> _userManager;
        private readonly IHttpContextAccessor _contextAccessor;
        public GetOtherTraineesQueryHandler(
            IUnitOfWork unitOfWork,
            UserManager<Account> userManager,
            IHttpContextAccessor contextAccessor)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _contextAccessor = contextAccessor;
        }

        public async Task<Response> Handle(GetOtherTraineesQuery query, CancellationToken cancellationToken)
        {
            var user = await _userManager.GetUserAsync(_contextAccessor.HttpContext!.User);

            if (user == null)
            {
                return await Response.FailureAsync("Unauthorized.", System.Net.HttpStatusCode.Unauthorized);
            }

            var head = await _unitOfWork.Heads.GetByIdAsync(user.Id);

            if (head is null)
            {
                return await Response.FailureAsync("Unauthorized.", System.Net.HttpStatusCode.Unauthorized);
            }

            var trainees = await GetOtherTrainees(query, head, cancellationToken);

            return await Response.SuccessAsync(trainees);
        }

        private async Task<List<GetOtherTraineesQueryDto>> GetOtherTrainees(GetOtherTraineesQuery query, HeadOfCamp head, CancellationToken cancellationToken)
        {
            var otherTrainees = await _unitOfWork.Trainees.Entities
                                            .Where(x => query.ToFilterIds.Contains(x.Id) && head.CampId == x.CampId)
                                            .Select(x => x.Account)
                                            .ToListAsync(cancellationToken);

            var traineesIds = otherTrainees.Select(x => x.Id).ToList();

            var attendance = await _unitOfWork.Repository<TraineeAttendence>().Entities
                            .Where(x => traineesIds.Contains(x.TraineeId))
                            .ToListAsync(cancellationToken);

            var sessionsCount = await _unitOfWork.Repository<Session>().Entities
                            .Where(x => x.CampId == head.CampId)
                            .CountAsync();

            var sheetsProblemsCount = await _unitOfWork.Repository<Sheet>().Entities
                        .Where(x => x.CampId == head.CampId && x.Status == SheetStatus.Completed)
                        .SumAsync(x => x.ProblemCount);

            var solutions = await _unitOfWork.Repository<TraineeAccessSheet>().Entities
                           .Where(x => traineesIds.Contains(x.TraineeId))
                           .ToListAsync(cancellationToken);

            var trainees = new List<GetOtherTraineesQueryDto>();

            foreach (var trainee in otherTrainees)
            {
                var absenceCount = sessionsCount - attendance.Count(x => x.TraineeId == trainee.Id);
                var traineeSolveCount = solutions.Where(x => x.TraineeId == trainee.Id).Count();

                trainees.Add(new()
                {
                    Id = trainee.Id,
                    AbsenceCount = absenceCount,
                    CodeForceHandle = trainee.CodeForceHandle,
                    Email = trainee.Email!,
                    FirstName = trainee.FirstName,
                    LastName = trainee.LastName,
                    MiddleName = trainee.MiddleName,
                    SolvingPrecent = (traineeSolveCount / sheetsProblemsCount) * 100
                });
            }

            return trainees;
        }
    }
}
