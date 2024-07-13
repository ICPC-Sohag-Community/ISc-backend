using ISc.Domain.Comman.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISc.Application.Features.HeadOfCamps.Sheets.Queries.GetAllWithPagination
{
	public class GetAllSheetsWithPaginationQueryDto
	{
		public string Name { get; set; }
		public string SheetLink { get; set; }
		public SheetStatus Status { get; set; }

	}
}
