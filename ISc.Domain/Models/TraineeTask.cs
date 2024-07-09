namespace ISc.Domain.Models
{
    public class TraineeTask : BaseEntity
    {
        public string Task { get; set; }
        public DateTime DeadLine { get; set; }
        public string TraineeId { get; set; }
    }
}
