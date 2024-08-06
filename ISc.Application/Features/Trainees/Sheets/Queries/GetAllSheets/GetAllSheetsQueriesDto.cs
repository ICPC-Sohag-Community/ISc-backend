using ISc.Domain.Comman.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISc.Application.Features.Trainees.Sheets.Queries.GetAllSheets
{
    public class GetAllSheetsQueriesDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string SheetLink { get; set; }
        public int ProblemCount { get; set; }
        public int ProblemSolved { get; set; }
        public int MinimumPassingPrecent { get; set; }
        public int SolvedPrecent { get; set; }
        public SheetStatus Status { get; set; }
        public DateOnly? Date { get; set; }
    }
}
