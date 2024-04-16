using ISc.Domain.Interface;

namespace ISc.Domain.Models
{
    public class Session : BaseEntity, ISoftEntity
    {
        public string Topic { get; set; }
        public string InstructorName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string LocationName { get; set; }
        public string LocationLink { get; set; }
        public int CampId { get; set; }
        public virtual Camp Camp { get; set; }
        public virtual ICollection<SessionFeedback> Feedbacks { get; set; }
    }
}
