using ISc.Domain.Comman.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISc.Application.Features.Leader.Staff.Queries.GetAllWithPagination
{
    public class GetAllStaffWithPaginationQueryDto
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public College College { get; set; }
        public int Grade { get; set; }
        public string CodeForceHandle { get; set; }
    }

    public enum GetAllWithPaginationQueryDtoColumns
    {
        Faculty,
        Grade,
        Gender
    }
}
