using ISc.Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISc.Domain.Models
{
    public class TraineeAccessContest:ISoftEntity
    {
        public string TraineeId { get; set; }
        public virtual Trainee Trainee { get; set; }
        public int ContestId { get; set; }
        public virtual Contest Contest { get; set; }
        public string Index { get; set; }
    }
}
