using ISc.Domain.Comman.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISc.Application.Features.HeadOfCamps.Contests.Queries.GetById
{
	public class GetContestByIdQueryDto
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Link { get; set; }
		public DateTime StartTime { get; set; }
		public DateTime EndTime { get; set; }
		public Community Community { get; set; }
		public string CodeForceId { get; set; }
		public int ProblemCount { get; set; }

	}
}
