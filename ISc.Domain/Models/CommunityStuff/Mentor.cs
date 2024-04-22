using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISc.Domain.Interface;
using ISc.Domain.Models.IdentityModels;

namespace ISc.Domain.Models.CommunityStuff
{
    public class Mentor : Account
    {
        public int? SessionId { get; set; }
        public virtual Session? Session { get; set; }
        public string? About { get; set; }
        public virtual ICollection<Trainee> Trainees { get; set; }
        public virtual ICollection<MentorsOfCamp> Camps { get; set; }
    }
}
