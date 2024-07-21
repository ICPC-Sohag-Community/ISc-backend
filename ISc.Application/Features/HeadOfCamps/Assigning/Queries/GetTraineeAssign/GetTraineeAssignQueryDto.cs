using ISc.Domain.Comman.Enums;

namespace ISc.Application.Features.HeadOfCamps.Assigning.Queries.GetTraineeAssign
{
    public class GetTraineeAssignQueryDto
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string? PhotoUrl { get; set; }
        public College College { get; set; }
        public Gender Gender { get; set; }
    }
    public enum SortBy
    {
        Grade,
        Gender,
        College
    }
}
