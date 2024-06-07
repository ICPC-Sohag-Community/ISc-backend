using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISc.Domain.Comman.Enums;

namespace ISc.Application.Features.Leader.Camps.Queries.GetCampEditById
{
    public class GetCampEditByIdQueryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateOnly startDate { get; set; }
        public DateOnly EndDate { get; set; }
        public Term? Term { get; set; }
        public int DurationInWeeks { get; set; }
        public bool OpenForRegister { get; set; }
        public List<GetCampMemeberEditByIdQueryDto> MentorsOfCamp { get; set; }
        public List<GetCampMemeberEditByIdQueryDto> HeadsOfCamp { get; set; }
    }

    public class GetCampMemeberEditByIdQueryDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool InCamp { get; set; }
    }
}
