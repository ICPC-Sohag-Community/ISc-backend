using ISc.Domain.Comman.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISc.Domain.Interface
{
    public interface IProblemContainers
    {
        public int Id { get; set; }
        public int ProblemCount { get; set; }
    }
}
