using ISc.Application.Interfaces;
using ISc.Application.Interfaces.Repos;
using ISc.Domain.Comman.Enums;
using ISc.Domain.Models;
using Microsoft.IdentityModel.Tokens;

namespace ISc.Infrastructure.Services.ScheduleTasks
{
    public class TrackingTraineesJob
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOnlineJudgeServices _onlineJudgeServices;

        public TrackingTraineesJob(
            IUnitOfWork unitOfWork,
            IOnlineJudgeServices onlineJudgeServices)
        {
            _unitOfWork = unitOfWork;
            _onlineJudgeServices = onlineJudgeServices;
        }

        public async Task UpdateTraineesSolving()
        {
            var camps = await _unitOfWork.Repository<Camp>().GetAllAsync();
            var solvedProblems = _unitOfWork.Repository<TraineeAccessSheet>()
                                    .Entities
                                    .ToHashSet();
            int requestCount = 1;

            foreach (var camp in camps)
            {
                var trainees = camp.Trainees.ToHashSet();

                var sheets = camp.Sheets.Where(x => x.EndDate <= DateOnly.FromDateTime(DateTime.Now))
                                        .Select(x => new
                                        {
                                            x.Id,
                                            x.SheetCodefroceId,
                                            x.MinimumPassingPrecent,
                                            x.ProblemCount,
                                            x.Community,
                                        }).ToHashSet();

                foreach (var sheet in sheets)
                {
                    foreach (var trainee in trainees)
                    {
                        var traineeHandle = trainee.Account.CodeForceHandle;

                        var sheetSolvedProblems = _onlineJudgeServices.GetGroupSheetStatusAsync(
                                                    sheetId: sheet.SheetCodefroceId,
                                                    count: 2700,
                                                    community: sheet.Community,
                                                    handle: traineeHandle
                                                    )!.Result.Where(x => x.verdict == "OK");

                        if (sheetSolvedProblems.IsNullOrEmpty())
                            continue;

                        var solved = solvedProblems
                                    .Where(x => x.SheetId == sheet.Id && x.TraineeId == trainee.Id)
                                    .Select(x => x.Index)
                                    .ToHashSet();

                        var newProblems = sheetSolvedProblems.Select(x => x.problem.index).ToHashSet();

                        if (newProblems.IsNullOrEmpty())
                            continue;

                        var newProblemsToDatabase = newProblems!.Except(solved)
                            .Select(x => new TraineeAccessSheet()
                            {
                                SheetId = sheet.Id,
                                TraineeId = trainee.Id,
                                Index = x
                            }).ToList();

                        trainee.Points += 20 * newProblems.Count;

                        await _unitOfWork.Trainees.UpdateAsync(new() { Account = trainee.Account, Member = trainee });

                        await _unitOfWork.Repository<TraineeAccessSheet>().AddRangeAsync(newProblemsToDatabase);

                        if (requestCount % 100 == 0)
                        {
                            Thread.Sleep(2000);
                        }
                        requestCount++;
                    }
                }
            }

            await _unitOfWork.SaveAsync();
        }
    }
}
