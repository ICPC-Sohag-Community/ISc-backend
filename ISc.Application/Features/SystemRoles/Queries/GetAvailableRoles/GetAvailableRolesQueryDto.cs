using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISc.Application.Features.SystemRoles.Queries.GetAvailableRoles
{
    public class GetAvailableRolesQueryDto
    {
        public string Role { get; set; }
        public List<GetAvailableRolesDependecyDto> AvailableCamps { get; set; }

    }
    public class GetAvailableRolesDependecyDto
    {
        public int CampId { get; set; }
        public string CampName { get; set; }
    }
}
