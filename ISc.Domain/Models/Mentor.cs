using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISc.Domain.Models
{
	public class Mentor:BaseEntity
	{
        public int? SessionId { get; set; }
        public string? About { get; set; }
    }
}
