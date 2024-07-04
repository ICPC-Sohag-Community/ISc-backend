using ISc.Application.Features.Leader.Reports.Queries.CampReoprts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace ISc.Application.Features.HeadOfCamps.Attendance.Queries.GetAllAttendance
{
    public class GetAllAttendanceQueryDto
    {
        public GetAllAttendanceQueryDto()
        {
            Sessions = new();
            Trainees = new();
        }

        public HashSet<AttendacneSessionInfoDto> Sessions { get; set; }
        public HashSet<AttendanceTraineeInfoDto> Trainees { get; set; }
    }

    public class AttendacneSessionInfoDto
    {
        public int Id { get; set; }
        public string Topic { get; set; }
    }

    public class AttendnaceTraineeSessionStatusDto
    {
        public int SheetId { get; set; }
        public bool? Status { get; set; }
    }

    public class AttendanceTraineeInfoDto
    {
        public string TraineeId { get; set; }
        public string Name { get; set; }
        public List<AttendnaceTraineeSessionStatusDto> Status { get; set; }
    }
}
