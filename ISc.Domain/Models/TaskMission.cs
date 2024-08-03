using ISc.Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISc.Domain.Models
{
    public class TaskMission:BaseEntity,ISoftEntity
    {
        public string Task { get; set; }
        public int TraineeTaskId { get; set; }
        public virtual TraineeTask TraineeTask { get; set; }
    }
}
