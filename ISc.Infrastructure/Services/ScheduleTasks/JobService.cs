using Hangfire;
using ISc.Application.Interfaces;
using ISc.Domain.Models;

namespace ISc.Infrastructure.Services.ScheduleTasks
{
    internal class JobService : IJobServices
    {
        public JobService()
        {
            new BackgroundJobServer();
        }

        public void TrackingTraineesSolving()
        {
            RecurringJob.AddOrUpdate<TrackingTraineesJob>("Update-Trainees-Progress", x => x.UpdateTraineesSolving(), "0 * * * *");
        }

        public void TrackingContest(Contest contest)
        {

            var executeTime = DateTimeOffset.Parse(contest.EndTime.ToString()!).AddMinutes(10);

            BackgroundJob.Schedule<RecordContestSolvedProblems>("Update-Trainees-Contest-Problems", x => x.Record(contest.Id), executeTime);
        }
    }
}