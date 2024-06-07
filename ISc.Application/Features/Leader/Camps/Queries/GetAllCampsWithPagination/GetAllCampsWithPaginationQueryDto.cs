using ISc.Domain.Comman.Enums;

namespace ISc.Application.Features.Leader.Camps.Queries.GetAllCampsWithPagination
{
    public class GetAllCampsWithPaginationQueryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateOnly startDate { get; set; }
        public DateOnly EndDate { get; set; }
        public Term? Term { get; set; }
        public int DurationInWeeks { get; set; }
    }
}
