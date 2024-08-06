using ISc.Domain.Comman.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISc.Application.Features.Trainees.Tasks.Queries.GetTaskWithStatus
{
    public class GetTaskWithStatusQueryDto
    {
        public TasksStatus Status { get; set; }
        public List<TaskDetailDto> Task { get; set; }
    }
    public class TaskDetailDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime DeadLine { get; set; }
    }
}
