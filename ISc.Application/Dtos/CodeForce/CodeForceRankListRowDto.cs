using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISc.Application.Dtos.CodeForce
{
    public class CodeForceRankListRowDto
    {
        public CodeForcePartyDto party { get; set; }
        public int rank { get; set; }

        public double points { get; set; }

        public List<CodeForceProblemResultDto> problemResults { get; set; }
    }
}
