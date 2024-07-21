using ISc.Domain.Comman.Enums;

namespace ISc.Application.Features.HeadOfCamps.Assigning.Queries.GetMentorAssign
{
    public class GetMentorAssignQueryDto
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public College College { get; set; }
        public Gender Gender { get; set; }
        public List<GetTraineeForMentorAssignDto> Trainees { get; set; }
    }
    public class GetTraineeForMentorAssignDto
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string? PhotoUrl { get; set; }
        public College College { get; set; }
        public Gender Gender { get; set; }
    }
}
