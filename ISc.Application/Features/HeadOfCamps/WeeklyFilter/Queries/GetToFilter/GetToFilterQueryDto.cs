using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISc.Application.Features.HeadOfCamps.WeeklyFilter.Queries.GetToFilter
{
    public class GetToFilterQueryDto
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string CodeForceHandle { get; set; }
        public int AbsenceCount { get; set; }
        public int WeeklySolvingPrecent { get; set; }
    }
}
