using ISc.Domain.Interface;
using ISc.Domain.Models.CommunityStaff;

namespace ISc.Domain.Models
{
    public class MentorsOfCamp : Auditable, ISoftEntity
    {
        public int CampId { get; set; }
        public virtual Camp Camp { get; set; }
        public string MentorId { get; set; }
        public virtual Mentor Mentor { get; set; }
    }
}
