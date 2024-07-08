using ISc.Domain.Comman.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISc.Application.Features.Leader.Archives.Queries.GetAllStaffsArchiveWithPagination
{
	public class GetAllStaffsArchiveWithPaginationQueryDto
	{
		public class GetAllTraineesArchiveWithPaginationQueryDto
		{
			public int Id { get; set; }
			public string FullName { get; set; }
			public string CodeForceHandle { get; set; }
			public Gender Gender { get; set; }
			public string CampName { get; set; }
			public string College { get; set; }
		}

		public enum filters
		{
			College,
			Gender,
			Grade
		}
	}
}
