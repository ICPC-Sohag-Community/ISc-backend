using ISc.Domain.Comman.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISc.Application.Features.Mentors.Tasks.Queries.GetTasksByStatus
{
    public class GetTasksByStatusQueryDto
    {
        public TasksStatus Status { get; set; }
        public List<GetTasksByStatusTraineeInfoDto> Trainees { get; set; }
    }

    public class GetTasksByStatusTraineeInfoDto
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string? PhotoUrl { get; set; }
        public string Task { get; set; }
        public DateTime DeadLine { get; set; }
    }

}
