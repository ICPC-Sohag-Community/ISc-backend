using ISc.Application.Interfaces.Repos;
using ISc.Domain.Comman.Enums;
using ISc.Domain.Models;
using ISc.Shared;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ISc.Application.Features.Leader.Reports.Queries.CampReoprts
{
    public record GetCampReoprtsQuery : IRequest<Response>;

    internal class GetCampReoprtsQueryHandler : IRequestHandler<GetCampReoprtsQuery, Response>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetCampReoprtsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Response> Handle(GetCampReoprtsQuery request, CancellationToken cancellationToken)
        {
            var entities = await _unitOfWork.Repository<Camp>().Entities
                        .ToListAsync(cancellationToken);

            var camps = new List<GetCampReoprtsQueryDto>();

            foreach (var entity in entities)
            {
                var camp = entity.Adapt<GetCampReoprtsQueryDto>();

                var trainees = await _unitOfWork.Trainees.Entities
                            .Where(x => x.CampId == camp.Id).Select(x => new TraineeInfoDto()
                            {
                                Id = x.Id,
                                Gender = x.Account.Gender,
                                College = x.Account.College,
                                Grade = x.Account.Grade
                            })
                            .ToListAsync(cancellationToken);

                camp.MaleCount = trainees.Count(x => x.Gender == Gender.Male);
                camp.FemaleCount = trainees.Count(x => x.Gender == Gender.Female);

                camp.TraineesGrades = trainees.GroupBy(x => x.Grade).Select(x => new TraineesInGradeDto()
                {
                    Grade = x.Key,
                    Count = x.Count()
                }).ToList();

                camp.TraineesColleges = trainees.GroupBy(x => x.College).Select(x => new TraineesInCollegeDto()
                {
                    College = x.Key,
                    Count = x.Count()
                }).ToList();

                GetsheetsAverage(ref camp, trainees);
                GetContestsAverage(ref camp, trainees);

                var sheetsCount = camp.SheetsRates.Count;

                camp.ProgressPrecent = sheetsCount > 0 ? (camp.SheetsRates.Select(x => x.Rate).Count() / camp.SheetsRates.Count) * 100 : 0;
            }

            return await Response.SuccessAsync(camps);
        }

        private void GetContestsAverage(ref GetCampReoprtsQueryDto camp, List<TraineeInfoDto> trainees)
        {
            var contestAverage = _unitOfWork.Repository<TraineeAccessContest>().Entities
                                .Where(x => trainees.Select(x => x.Id).Contains(x.TraineeId))
                                .OrderBy(x=>x.Contest.StartTime)
                                .GroupBy(x => x.Contest).Select(x => new
                                {
                                    Contest = x.Key,
                                    ProblemsCount = x.Count()
                                });

            foreach (var contestInfo in contestAverage)
            {
                var contest = contestInfo.Contest;
                camp.ContestRates.Add(new()
                {
                    Id = contest.Id,
                    Name = contest.Name,
                    Rate = contest.ProblemCount > 0 ? Math.Ceiling((double)contestInfo.ProblemsCount / contest.ProblemCount) * 100 : 0
                });
            }
        }

        private void GetsheetsAverage(ref GetCampReoprtsQueryDto camp, List<TraineeInfoDto> trainees)
        {
            var sheetsAverage = _unitOfWork.Repository<TraineeAccessSheet>().Entities
                                               .Where(x => trainees.Select(x => x.Id).Contains(x.TraineeId))
                                               .OrderBy(x=>x.Sheet.SheetOrder)
                                               .GroupBy(x => x.Sheet).Select(x => new
                                               {
                                                   Sheet = x.Key,
                                                   ProblemsCount = x.Count()
                                               });

            foreach (var sheetInfo in sheetsAverage)
            {
                var sheet = sheetInfo.Sheet;
                camp.SheetsRates.Add(new()
                {
                    Id = sheet.Id,
                    Name = sheet.Name,
                    Rate = sheet.ProblemCount > 0 ? Math.Ceiling((double)sheetInfo.ProblemsCount / sheet.ProblemCount) * 100 : 0
                });
            }
        }
    }
}
