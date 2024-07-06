using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISc.Application.Features.SystemRoles.Queries.GetUserRoles
{
    public class GetUserRolesQueryDto
    {
        public string RoleId { get; set; }
        public string Role { get; set; }
        public int?  CampId { get; set; }
        public string? CampName { get; set;}
    }
}
