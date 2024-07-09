using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISc.Application.Features.Leader.Dashboard.Queries.GetFeedbacks
{
	public class GetFeedbacksQueryDto
	{
		public int Rate { get; set; }
		public string? Feedback { get; set; }
		public string? PhotoUrl { get; set; }
		public string FullName { get; set; }
	}
}
