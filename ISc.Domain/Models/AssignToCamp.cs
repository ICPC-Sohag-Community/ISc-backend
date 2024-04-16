using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISc.Domain.Interface;

namespace ISc.Domain.Models
{
	public class AssignToCamp:Auditable,ISoftEntity
	{
        public int CampId { get; set; }
        public string MentorId { get; set; }
    }
}
