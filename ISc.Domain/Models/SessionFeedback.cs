using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISc.Domain.Models
{
	public class SessionFeedback
	{
        public int SessionId { get; set; }
        public string TraineeId { get; set; }
        public int Rate { get; set; }
        public string? Feedback { get; set; }
    }
}
