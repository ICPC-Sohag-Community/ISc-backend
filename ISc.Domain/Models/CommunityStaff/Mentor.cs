using ISc.Domain.Models.IdentityModels;

namespace ISc.Domain.Models.CommunityStaff
{
    public class Mentor : Staff
    {
        public virtual Account Account { get; set; }
        public string? About { get; set; }
        public virtual ICollection<Trainee> Trainees { get; set; }
        public virtual ICollection<MentorsOfCamp> Camps { get; set; }
        public virtual ICollection<Practice> Practices { get; set; }
    }
}
