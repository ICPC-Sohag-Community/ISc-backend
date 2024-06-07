using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISc.Domain.Comman.Enums;

namespace ISc.Application.Features.Leader.Request.Queries.DisplayAll
{
    public class DisplayAllRegisterationQueryDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public int Grade { get; set; }
        public College College { get; set; }
        public Gender Gender { get; set; }
        public string? Comment { get; set; }
        public bool HasLaptop { get; set; }
        public string CodeForceHandle { get; set; }
    }
}
