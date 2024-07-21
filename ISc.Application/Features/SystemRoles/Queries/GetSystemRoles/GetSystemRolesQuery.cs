using ISc.Shared;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISc.Application.Features.SystemRoles.Queries.GetSystemRoles
{
    public record GetSystemRolesQuery:IRequest<Response>;

    internal class GetSystemRolesDto : IRequestHandler<GetSystemRolesQuery, Response>
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public GetSystemRolesDto(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<Response> Handle(GetSystemRolesQuery request, CancellationToken cancellationToken)
        {
            var roles = await _roleManager.Roles
                               .ProjectToType<GetSystemRolesQueryDto>()
                               .ToListAsync(cancellationToken);

            return await Response.SuccessAsync(roles);
        }
    }
}
