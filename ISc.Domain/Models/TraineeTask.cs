namespace ISc.Domain.Models
{
    public class TraineeTask : BaseEntity
    {
        public int TraineeId { get; set; }
        public string Task { get; set; }
        public DateTime DeadLine { get; set; }
    }
}
