using ISc.Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISc.Domain.Models
{
	public class SessionFeedback:Auditable,ISoftEntity
	{
        public int SessionId { get; set; }
        public virtual Session Session { get; set; }
        public string TraineeId { get; set; }
        public virtual Trainee Trainee { get; set; }
        public int Rate { get; set; }
        public string? Feedback { get; set; }
    }
}
