using ISc.Domain.Comman.Enums;
using ISc.Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISc.Domain.Models
{
    public class Contest:BaseEntity,ISoftEntity
    {
        public string Name { get; set; }
        public string Link { get; set; }
        public Community Community { get; set; }
        public int ProblemCount { get; set; }
        public string CodeForceId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int CampId { get; set; }
        public virtual Camp Camp { get; set; }
        public virtual ICollection<TraineeAccessContest> Trainees { get; set; }
    }
}
