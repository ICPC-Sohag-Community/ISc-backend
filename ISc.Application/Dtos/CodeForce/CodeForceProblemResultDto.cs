using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISc.Application.Dtos.CodeForce
{
    public class CodeForceProblemResultDto
    {
        public double points { get; set; }
        public long? penalty { get; set; }
        public int rejectedAttemptCount { get; set; }
        public int? bestSubmissionTimeSeconds { get; set; }
    }
}
