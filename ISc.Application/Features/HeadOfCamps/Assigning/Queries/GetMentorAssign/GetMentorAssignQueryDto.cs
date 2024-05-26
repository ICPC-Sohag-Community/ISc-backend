using ISc.Domain.Comman.Enums;

namespace ISc.Application.Features.HeadOfCamps.Assigning.Queries.GetMentorAssign
{
    public class GetMentorAssignQueryDto
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public College College { get; set; }
        public Gender Gender { get; set; }
        public List<GetTraineeForMentorAssign> Trainees { get; set; }
    }
    public class GetTraineeForMentorAssign
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string? PhotoUrl { get; set; }
        public College College { get; set; }
        public Gender Gender { get; set; }
        public string CodeForceHandle { get; set; }
        public string? MentorId { get; set; }
    }
}
