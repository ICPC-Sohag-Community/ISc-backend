using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISc.Application.Dtos.CodeForce
{
    public class CodeForcePartyDto
    {
        public int? contestId { get; set; }
        public List<CodeForceMemberDto> members { get; set; }
        public string participantType { get; set; }
    }
}
