using ISc.Application.Features.Mentors.Sheets.Queries.TraineesSheetsQuery;
using ISc.Application.Interfaces.Repos;
using ISc.Domain.Comman.Constant;
using ISc.Domain.Models;
using ISc.Shared;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISc.Application.Features.Mentors.Contests.Queries.GetTraineesContests
{
    public record GetTraineesContestsQuery:IRequest<Response>
    {
        public int CampId { get; set; }

        public GetTraineesContestsQuery(int campId)
        {
            CampId = campId;
        }
    }

    internal class GetMentorsContestsQueryHandler : IRequestHandler<GetTraineesContestsQuery, Response>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetMentorsContestsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Response> Handle(GetTraineesContestsQuery query, CancellationToken cancellationToken)
        {
            var trainees = await _unitOfWork.Trainees.Entities.Where(x => x.CampId == query.CampId)
                          .Select(x => x.Account)
                          .ProjectToType<TraineeDto>()
                          .ToListAsync(cancellationToken);

            var contests = await _unitOfWork.Repository<Contest>().Entities
                           .Where(x => x.CampId == query.CampId && x.StartTime.AddMinutes(Constrains.PeriodTimeForUpdateContest) <= DateTime.Now)
                           .ProjectToType<ContestDto>()
                           .ToListAsync(cancellationToken);

            var tracking = _unitOfWork.Repository<TraineeAccessContest>().Entities
                        .Where(x => x.Contest.Camp.Id == query.CampId)
                        .GroupBy(x => new { x.TraineeId, x.ContestId })
                        .Select(x => new
                        {
                            x.Key.ContestId,
                            x.Key.TraineeId,
                            Count = x.Count()
                        })
                        .ToHashSet();

            foreach(var trainee in trainees)
            {
                foreach(var contest in contests)
                {
                    var traineeTracking = tracking.SingleOrDefault(x => x.ContestId == contest.Id && x.TraineeId == trainee.Id);

                    if(traineeTracking is not null)
                    {
                        trainee.Tracking.Add(new()
                        {
                            ContestId = contest.Id,
                            SolvedProblems = traineeTracking.Count
                        });
                    }
                }
            }

            return await Response.SuccessAsync(new GetTraineesContestsQueryDto()
            {
                Contests = contests,
                Trainees = trainees
            });
        }
    }
}
