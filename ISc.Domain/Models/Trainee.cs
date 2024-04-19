using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISc.Domain.Models.CommunityStuff;
using ISc.Domain.Models.IdentityModels;

namespace ISc.Domain.Models
{
    public class Trainee : Account
    {
        public int CampId { get; set; }
        public virtual Camp Camp { get; set; }
        public string? MentorId { get; set; }
        public virtual Mentor Mentor { get; set; }
        public int Points { get; set; }
        public virtual ICollection<TraineeTask> Tasks { get; set; }
    }
}
