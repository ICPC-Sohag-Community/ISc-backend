using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISc.Application.Dtos.CodeForce
{
    internal class CodeForceStandingDto
    {
        public CodeForceContestDto contest { get; set; }
        public List<CodeForceProblemDto> problems { get; set; }
        public List<CodeForceRankListRowDto> rows { get; set; }
    }
}
