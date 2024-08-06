using ISc.Domain.Comman.Enums;

namespace ISc.Application.Features.Trainees.Sheets.Queries.GetCurrentSheet
{
    public class GetCurrentSheetQueryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string SheetLink { get; set; }
        public int ProblemCount { get; set; }
        public DateOnly? EndDate { get; set; }
    }
}
