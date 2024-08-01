using ISc.Domain.Comman.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISc.Application.Features.Mentors.Practices.Queries.GetPractice
{
    public class GetPracticeQueryDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string MeetingLink { get; set; }
        public string Note { get; set; }
        public DateTime Time { get; set; }
        public PracticeStatus State { get; set; }
    }
}
