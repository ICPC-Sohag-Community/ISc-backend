using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISc.Application.Features.Trainees.Contests.Queries.GetInComingContest
{
    public class GetInComingContestQueryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
        public RemainTimeDto RemainTime { get; set; }
        public bool IsRunning { get; set; }
    }
    public class RemainTimeDto
    {
        public int Days { get; set; }
        public int Hours { get; set; }
        public int Minutes { get; set; }
        public int Seconds { get; set; }
    }
}
