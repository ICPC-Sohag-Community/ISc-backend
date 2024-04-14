using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISc.Application.Dtos.CodeForce
{
    public class CodeForceProblemDto
    {
        public int? contestId { get; set; }
        public string index { get; set; }
        public string name { get; set; }
        public int? rating { get; set; }
    }
}
