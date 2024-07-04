using ISc.Domain.Comman.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISc.Application.Features.Leader.Reports.Queries.CampReoprts
{
    public class TraineeInfoDto
    {
        public string Id { get; set; }
        public Gender Gender { get; set; }
        public College College { get; set; }
        public int Grade { get; set; }

    }
}
