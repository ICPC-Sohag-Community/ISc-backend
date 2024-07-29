using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISc.Application.Features.Mentors.Contests.Queries.GetTraineesContests
{
    public class GetTraineesContestsQueryDto
    {
        public List<ContestDto> Contests { get; set; }
        public List<TraineeDto> Trainees { get; set; }
    }

    public class ContestDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
        public int ProblemCount { get; set; }
    }

    public class TraineeDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<Tracking> Tracking { get; set; }
    }

    public class Tracking 
    {
        public int ContestId { get; set; }
        public int SolvedProblems { get; set; }
    }
}
