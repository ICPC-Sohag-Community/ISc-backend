using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISc.Application.Dtos.Standing
{
    public class StandingDto
    {
        public int TotalProblems { get; set; }
        public List<AchiverDto>Achivers { get; set; }
    }
    public class AchiverDto
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string CodeForceHandle { get; set; }
        public int SolvedProblems { get; set; }
        public int Points { get; set; }
        public string MentorName { get; set; }
    }
}
