using ISc.Domain.Comman.Enums;

namespace ISc.Application.Features.HeadOfCamps.Sheets.Queries.GetById
{
    public class GetSheetByIdQueryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string SheetLink { get; set; }
        public int MinimumPassingPrecent { get; set; }
        public int SheetOrder { get; set; }
        public Community Community { get; set; }
        public int ProblemCount { get; set; }
        public string SheetCodefroceId { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public SheetStatus Status { get; set; }
    }
}
