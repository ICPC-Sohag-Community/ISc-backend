using ISc.Domain.Comman.Enums;
using ISc.Domain.Interface;

namespace ISc.Domain.Models
{
    public class TraineeTask : BaseEntity,ISoftEntity
    {
        public string Task { get; set; }
        public DateTime DeadLine { get; set; }
        public TasksStatus Status { get; set; }
        public string TraineeId { get; set; }
        public virtual Trainee Trainee { get; set; }
    }
}
