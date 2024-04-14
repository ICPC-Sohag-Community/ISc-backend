using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISc.Application.Dtos.CodeForce
{
    public class CodeForceBaseResponseDto<T>
    {
        public string status { get; set; }
        public string? comment { get; set; }
        public T result { get; set; }
    }
}
