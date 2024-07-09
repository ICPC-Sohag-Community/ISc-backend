using ISc.Application.Interfaces;
using ISc.Application.Interfaces.Repos;
using ISc.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace ISc.Infrastructure.Services.ScheduleTasks
{
    public class RecordContestSolvedProblems
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOnlineJudgeServices _onlineJudgeServices;
        private readonly ILogger<RecordContestSolvedProblems> _logger;

        public RecordContestSolvedProblems(
            IUnitOfWork unitOfWork,
            ILogger<RecordContestSolvedProblems> logger,
            IOnlineJudgeServices onlineJudgeServices)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _onlineJudgeServices = onlineJudgeServices;
        }

        public async Task Record(int contestId)
        {
            var contest = await _unitOfWork.Repository<Sheet>().GetByIdAsync(contestId);

            if (contest is null)
            {
                _logger.LogInformation($"contest id: {contestId} is not accessable or not found");
                return;
            }

            var trainees = await _unitOfWork.Trainees.Entities
                        .Where(x => x.CampId == contest.CampId)
                        .Select(x => new
                        {
                            x.Account.CodeForceHandle,
                            x.Id
                        })
                        .ToListAsync();

            var status = _onlineJudgeServices.GetGroupSheetStatusAsync(contestId.ToString(), 2000, contest.Community)!.Result?.ToHashSet();

            if (status == null)
            {
                _logger.LogInformation($"Contest with id:{contestId} has no status");
                return;
            }

            foreach (var trainee in trainees)
            {
                var traineeStatus = status.Where(x => x.author.members.First().handle == trainee.CodeForceHandle && x.verdict == "OK")
                    .Select(x => new TraineeAccessSheet()
                    {
                        TraineeId = trainee.Id,
                        SheetId = contest.Id,
                        AccessDate = DateOnly.FromDateTime(DateTime.Now),
                        Index = x.problem.index
                    }).ToList();

                if (!traineeStatus.IsNullOrEmpty())
                {
                    await _unitOfWork.Repository<TraineeAccessSheet>().AddRangeAsync(traineeStatus);
                }
            }

            await _unitOfWork.SaveAsync();
        }
    }
}
