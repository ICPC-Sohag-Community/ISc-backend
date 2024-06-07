using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hangfire;
using ISc.Application.Interfaces;

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
            RecurringJob.AddOrUpdate<TrackingTraineesJob>("Trainees-daily-task",x=>x.UpdateTraineesSolving(), "0 * * * *");
        }
    }
}
