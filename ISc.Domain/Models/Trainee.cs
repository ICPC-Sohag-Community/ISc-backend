using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISc.Domain.Models
{
	public class Trainee
	{
        public string Id { get; set; }
        public int  CampId { get; set; }
        public string? MentorId { get; set; }
        public int Points { get; set; }
    }
}
