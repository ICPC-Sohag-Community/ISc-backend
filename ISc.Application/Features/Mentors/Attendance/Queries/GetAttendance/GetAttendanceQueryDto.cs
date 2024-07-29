namespace ISc.Application.Features.Mentors.Attendance.Queries.GetAttendance
{
    public class GetAttendanceQueryDto
    {
        public GetAttendanceQueryDto()
        {
            Sessions = [];
            Trainees = [];
        }

        public List<GetAttendanceSessionDto> Sessions { get; set; }
        public List<GetAttendanceTraineeDto> Trainees { get; set; }
    }

    public class GetAttendanceSessionDto
    {
        public int Id { get; set; }
        public string Topic { get; set; }
    }
    public class GetAttendanceTraineeDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<GetAttendanceDto> Attendances { get; set; } = [];
    }
    public class GetAttendanceDto
    {
        public int SessionId { get; set; }
        public bool State { get; set; }
    }
}
