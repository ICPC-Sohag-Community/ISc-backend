using ISc.Application.Interfaces.Repos;
using ISc.Domain.Comman.Enums;
using ISc.Domain.Models;
using ISc.Domain.Models.IdentityModels;
using ISc.Shared;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace ISc.Application.Features.HeadOfCamps.WeeklyFilter.Queries.GetToFilter
{
    public record GetToFilterQuery : IRequest<Response>;

    internal class GetToFilterQueryHandler : IRequestHandler<GetToFilterQuery, Response>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly UserManager<Account> _userManager;

        public GetToFilterQueryHandler(
            IUnitOfWork unitOfWork,
            IHttpContextAccessor contextAccessor,
            UserManager<Account> userManager)
        {
            _unitOfWork = unitOfWork;
            _contextAccessor = contextAccessor;
            _userManager = userManager;
        }

        public async Task<Response> Handle(GetToFilterQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.GetUserAsync(_contextAccessor.HttpContext!.User);

            if (user is null)
            {
                return await Response.FailureAsync("Unauthorized.", HttpStatusCode.Unauthorized);
            }

            var head = await _unitOfWork.Heads.GetByIdAsync(user.Id);

            if (head is null)
            {
                return await Response.FailureAsync("User not a head for any camp.", HttpStatusCode.BadRequest);
            }

            var trainees = await _unitOfWork.Trainees.Entities
                        .Where(x => x.CampId == head.CampId)
                        .Select(x => new
                        {
                            x.Id,
                            x.Account.FirstName,
                            x.Account.MiddleName,
                            x.Account.LastName,
                            x.Account.Email,
                            x.Account.CodeForceHandle
                        })
                        .ToListAsync(cancellationToken);

            var attendances = await _unitOfWork.Repository<TraineeAttendence>().Entities
                            .Where(x => trainees.Select(t => t.Id).Contains(x.TraineeId))
                            .ToListAsync(cancellationToken);

            var sheets = await _unitOfWork.Repository<Sheet>().Entities
                        .Where(x => x.CampId == head.CampId && x.Status == SheetStatus.Completed)
                        .Select(x => new
                        {
                            x.Id,
                            x.MinimumPassingPrecent,
                            x.ProblemCount
                        })
                        .ToListAsync(cancellationToken);

            var traineesSolutions = await _unitOfWork.Repository<TraineeAccessSheet>().Entities
                                    .Where(x => trainees.Select(t => t.Id).Contains(x.TraineeId))
                                    .ToListAsync(cancellationToken);

            var toFilter = new List<GetToFilterQueryDto>();
            var minimumAttendance = attendances.Select(x => x.SessionId).Distinct().Count() - 3;

            foreach (var trainee in trainees)
            {
                var attendanceCount = attendances.Count(x => x.TraineeId == trainee.Id);

                bool isFilter = attendanceCount < minimumAttendance;

                var traineeSolutions = traineesSolutions.Where(x => x.TraineeId == trainee.Id)
                                                        .GroupBy(x => x.SheetId)
                                                        .Select(x => new
                                                        {
                                                            SheetId = x.Key,
                                                            SolveCount = x.Count()
                                                        });
                int totalSolved = 0, problemsCount = 0;

                foreach (var sheet in sheets)
                {
                    var solution = traineeSolutions.FirstOrDefault(x => x.SheetId == sheet.Id);

                    if (solution == null)
                    {
                        isFilter = true;
                        break;
                    }

                    totalSolved += solution.SolveCount;
                    problemsCount += sheet.ProblemCount;

                    isFilter = solution == null || Convert.ToInt32(Math.Ceiling((double)solution.SolveCount / sheet.ProblemCount) * 100) < minimumAttendance;

                    if (isFilter == true)
                    {
                        break;
                    }
                }

                if (isFilter == true)
                {
                    toFilter.Add(new()
                    {
                        Id = trainee.Id,
                        FirstName = trainee.FirstName,
                        MiddleName = trainee.MiddleName,
                        LastName = trainee.LastName,
                        CodeForceHandle = trainee.CodeForceHandle,
                        Email = trainee.Email,
                        AbsenceCount = (minimumAttendance + 3) - attendanceCount,
                        WeeklySolvingPrecent = (totalSolved / problemsCount) * 100
                    });
                }
            }

            return await Response.SuccessAsync(toFilter);
        }
    }
}