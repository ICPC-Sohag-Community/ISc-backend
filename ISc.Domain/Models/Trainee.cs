using ISc.Domain.Models.CommunityStuff;
using ISc.Domain.Models.IdentityModels;

namespace ISc.Domain.Models
{
    public class Trainee
    {
        public string Id { get; set; }
        public virtual Account Account { get; set; }
        public int CampId { get; set; }
        public virtual Camp Camp { get; set; }
        public string? MentorId { get; set; }
        public virtual Mentor? Mentor { get; set; }
        public int Points { get; set; }
        public virtual ICollection<TraineeTask> Tasks { get; set; }
        public virtual ICollection<TraineeAttendence> Attendences { get; set; }
        public virtual ICollection<SessionFeedback> SessionFeedbacks { get; set; }
    }
}
