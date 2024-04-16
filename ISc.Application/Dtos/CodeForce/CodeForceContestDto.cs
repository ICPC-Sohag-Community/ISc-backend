using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISc.Application.Dtos.CodeForce
{
    public class CodeForceContestDto
    {
        public int id { get; set; }
        public string name { get; set; }
        public string? preparedBy { get; set; }
        public int? difficulty { get; set; }
    }
}
