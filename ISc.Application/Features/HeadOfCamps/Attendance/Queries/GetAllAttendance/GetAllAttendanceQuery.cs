using ISc.Application.Interfaces.Repos;
using ISc.Domain.Models;
using ISc.Domain.Models.IdentityModels;
using ISc.Shared;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using System.Net;

namespace ISc.Application.Features.HeadOfCamps.Attendance.Queries.GetAllAttendance
{
    public record GetAllAttendanceQuery : IRequest<Response>;

    internal class GetAllAttendanceQueryHandler : IRequestHandler<GetAllAttendanceQuery, Response>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<Account> _userManager;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IMemoryCache _memoryCache;
        public GetAllAttendanceQueryHandler(
            IUnitOfWork unitOfWork,
            UserManager<Account> userManager,
            IHttpContextAccessor contextAccessor,
            IMemoryCache memoryCache)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _contextAccessor = contextAccessor;
            _memoryCache = memoryCache;
        }

        public async Task<Response> Handle(GetAllAttendanceQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.GetUserAsync(_contextAccessor.HttpContext!.User);

            if (user == null)
            {
                return await Response.FailureAsync("Unauthorized.", HttpStatusCode.Unauthorized);
            }

            var campId = _unitOfWork.Heads.GetByIdAsync(user.Id).Result?.CampId;

            if (campId == null)
            {
                return await Response.FailureAsync("User not subscribe for any camp.", HttpStatusCode.NotFound);
            }

            var traineeAttendance = await _memoryCache.GetOrCreateAsync($"CampAttendance{campId}", cache =>
            {
                cache.SlidingExpiration = TimeSpan.FromMinutes(1);
                GetAllAttendanceQueryDto attendance = GetAttendance(campId.Value);

                cache.Value = attendance;
                return Task.FromResult(attendance);
            });

            return await Response.SuccessAsync(traineeAttendance!);
        }

        private GetAllAttendanceQueryDto GetAttendance(int campId)
        {
            var trainees = _unitOfWork.Trainees.Entities
                                        .Where(x => x.CampId == campId)
                                        .Select(x => new
                                        {
                                            Name = x.Account.FirstName + ' ' + x.Account.MiddleName + ' ' + x.Account.LastName,
                                            x.Id
                                        }).ToHashSet();

            var sessions = _unitOfWork.Repository<Session>().Entities
                            .Where(x => x.CampId == campId)
                            .Select(x => new
                            {
                                x.Id,
                                x.Topic,
                                x.StartDate
                            }).ToHashSet();

            var traineesAttendence = _unitOfWork.Repository<TraineeAttendence>()
                                           .Entities.Where(x => x.Session.CampId == campId)
                                           .ToHashSet();

            var attendance = new GetAllAttendanceQueryDto
            {
                Sessions = sessions.Adapt<HashSet<AttendacneSessionInfoDto>>()
            };

            var incomingSessions = attendance.Sessions
                                  .Select(x => x.Id)
                                  .Where(x => !traineesAttendence.Select(ta => ta.SessionId).Contains(x))
                                  .ToHashSet();

            foreach (var trainee in trainees)
            {
                var attendAttendance = new AttendanceTraineeInfoDto()
                {
                    TraineeId = trainee.Id,
                    Name = trainee.Name,
                    Status = []
                };

                foreach (var session in sessions)
                {
                    var traineeStatus = new AttendnaceTraineeSessionStatusDto()
                    {
                        SheetId = session.Id
                    };

                    var isAttend = traineesAttendence.Any(x => x.TraineeId == trainee.Id && x.SessionId == session.Id);

                    var isCompletedSession = traineesAttendence.Any(x => x.SessionId == session.Id);

                    if (isCompletedSession)
                    {
                        traineeStatus.Status = isAttend;
                    }

                    attendAttendance.Status.Add(traineeStatus);
                }

                attendance.Trainees.Add(attendAttendance);
            }

            return attendance;
        }
    }
}
