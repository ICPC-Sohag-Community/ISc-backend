using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISc.Application.Dtos.CodeForce
{
    public class CodeForceSubmissionDto
    {
        public int id { get; set; }
        public int? contestId { get; set; }
        public CodeForcePartyDto author { get; set; }
        public CodeForceProblemDto problem { get; set; }
        public string? verdict { get; set; }
        public int? creationTimeSeconds { get; set; }
    }
}
