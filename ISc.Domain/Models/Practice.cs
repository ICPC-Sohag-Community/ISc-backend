using ISc.Domain.Comman.Enums;
using ISc.Domain.Interface;
using ISc.Domain.Models.CommunityStaff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISc.Domain.Models
{
    public class Practice:BaseEntity,ISoftEntity
    {
        public string MeetingLink { get; set; }
        public string Note { get; set; }
        public DateTime Time { get; set; }
        public PracticeState State { get; set; }
        public int CampId { get; set; }
        public virtual Camp Camp { get; set; }
        public string MentorId { get; set; }
        public virtual Mentor Mentor { get; set; }
    }
}
