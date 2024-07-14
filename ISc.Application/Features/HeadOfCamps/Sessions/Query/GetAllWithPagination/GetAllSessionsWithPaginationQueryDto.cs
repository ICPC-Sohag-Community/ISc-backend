using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISc.Application.Features.HeadOfCamps.Sessions.Query.GetAllWithPagination
{
	public class GetAllSessionsWithPaginationQueryDto
	{
		public int Id { get; set; }
		public string Topic { get; set; }
		public string InstructorName { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public string LocationName { get; set; }
	}
}
