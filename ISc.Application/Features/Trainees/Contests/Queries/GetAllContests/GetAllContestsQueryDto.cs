using ISc.Domain.Comman.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISc.Application.Features.Trainees.Contests.Queries.GetAllContests
{
    public class GetAllContestsQueryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
        public DateTime Date { get; set; }
        public bool IsComming { get; set; }
    }
}
