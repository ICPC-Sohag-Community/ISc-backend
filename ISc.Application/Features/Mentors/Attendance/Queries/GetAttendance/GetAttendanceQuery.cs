using ISc.Application.Interfaces.Repos;
using ISc.Domain.Models;
using ISc.Domain.Models.IdentityModels;
using ISc.Shared;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISc.Application.Features.Mentors.Attendance.Queries.GetAttendance
{
    public record GetAttendanceQuery : IRequest<Response>
    {
        public int CampId { get; set; }

        public GetAttendanceQuery(int campId)
        {
            CampId = campId;
        }
    }

    internal class GetAttendanceQueryHandler : IRequestHandler<GetAttendanceQuery, Response>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly UserManager<Account> _userManager;

        public GetAttendanceQueryHandler(
            IUnitOfWork unitOfWork,
            IHttpContextAccessor contextAccessor,
            UserManager<Account> userManager)
        {
            _unitOfWork = unitOfWork;
            _contextAccessor = contextAccessor;
            _userManager = userManager;
        }

        public async Task<Response> Handle(GetAttendanceQuery query, CancellationToken cancellationToken)
        {
            var mentor = await _userManager.GetUserAsync(_contextAccessor.HttpContext!.User);

            if (mentor == null)
            {
                return await Response.FailureAsync("Unauthorized.", System.Net.HttpStatusCode.Unauthorized);
            }

            var attendance = new GetAttendanceQueryDto();

            var trainees = await _unitOfWork.Trainees.Entities
                        .Where(x => x.CampId == query.CampId)
                        .ToListAsync(cancellationToken);

            var sessions = await _unitOfWork.Repository<Session>().Entities
                                .Where(s => s.CampId == query.CampId)
                                .ProjectToType<GetAttendanceSessionDto>()
                                .ToListAsync(cancellationToken);

            attendance.Sessions.AddRange(sessions);

            var traineesAttendance = _unitOfWork.Repository<TraineeAttendence>().Entities
                                    .Where(x => x.Session.CampId == query.CampId)
                                    .ToHashSet();
            
            foreach(var trainee in trainees)
            {
                var attendedSessions = traineesAttendance.Where(x => x.TraineeId == trainee.Id)
                    .Select(x => new GetAttendanceDto()
                    {
                        SessionId = x.SessionId,
                        State = true
                    }).ToList();

                var absenseSessions = traineesAttendance
                                       .Where(x => x.TraineeId != trainee.Id)
                                       .ToList()
                                       .Adapt<List<GetAttendanceDto>>();

                attendance.Trainees.Add(new()
                {
                    Id = trainee.Id,
                    Attendances = attendedSessions.Concat(absenseSessions).ToList(),
                });
            }

            return await Response.SuccessAsync(attendance);
        }
    }
}
