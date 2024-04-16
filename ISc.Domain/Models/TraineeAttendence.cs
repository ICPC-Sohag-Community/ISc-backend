using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISc.Domain.Interface;

namespace ISc.Domain.Models
{
	public class TraineeAttendence:Auditable,ISoftEntity
	{
        public int TaineeId { get; set; }
        public int SessionId { get; set; }
    }
}
