using ISc.Domain.Comman.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISc.Application.Features.Mentors.TraineesInfo.Queries.GetTraineeInDetail
{
    public class GetTraineeInDetailQueryDto
    {
        public int Grade { get; set; }
        public string? VjudgeHandle { get; set; }
        public string Email { get; set; }
        public Gender Gender { get; set; }
    }
}
