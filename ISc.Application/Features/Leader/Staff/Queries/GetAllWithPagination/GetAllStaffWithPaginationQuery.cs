using ISc.Application.Extension;
using ISc.Domain.Comman.Constant;
using ISc.Domain.Models.IdentityModels;
using ISc.Shared;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace ISc.Application.Features.Leader.Staff.Queries.GetAllWithPagination
{
    public record GetAllStaffWithPaginationQuery : PaginatedRequest, IRequest<PaginatedRespnose<GetAllStaffWithPaginationQueryDto>>
    {
        public GetAllWithPaginationQueryDtoColumns? SortBy { get; set; }
    }

    internal class GetAllWithPaginationQueryHandler : IRequestHandler<GetAllStaffWithPaginationQuery, PaginatedRespnose<GetAllStaffWithPaginationQueryDto>>
    {
        private readonly UserManager<Account> _userManager;
        public GetAllWithPaginationQueryHandler(UserManager<Account> userManager)
        {
            _userManager = userManager;
        }

        public async Task<PaginatedRespnose<GetAllStaffWithPaginationQueryDto>> Handle(GetAllStaffWithPaginationQuery query, CancellationToken cancellationToken)
        {
            var accounts = await _userManager.Users.ToListAsync(cancellationToken);

            var staff = new List<Account>();

            foreach (var user in accounts)
            {
                var userRoles = await _userManager.GetRolesAsync(user);

                if (userRoles != null && userRoles.Any(x => x != Roles.Trainee))
                {
                    staff.Add(user);
                }
            }

            if (!query.KeyWord.IsNullOrEmpty())
            {
                staff = staff.Where(x => (x.FirstName + x.MiddleName + x.LastName).Trim().ToLower().StartsWith(query.KeyWord!.Trim().ToLower()))
                           .Where(x => x.Email!.ToLower().StartsWith(query.KeyWord!.ToLower()))
                           .Where(x => x.CodeForceHandle.ToLower().StartsWith(query.KeyWord!.ToLower()))
                           .ToList();
            }

            if (query.SortBy != null)
            {
                if (query.SortBy == GetAllWithPaginationQueryDtoColumns.Faculty)
                {
                    staff = staff.OrderBy(x => x.College).ToList();
                }
                else if (query.SortBy == GetAllWithPaginationQueryDtoColumns.Grade)
                {
                    staff = staff.OrderBy(x => x.Grade).ToList();
                }
                else if (query.SortBy == GetAllWithPaginationQueryDtoColumns.Gender)
                {
                    staff = staff.OrderBy(x => x.Gender).ToList();
                }
            }
            var entities = staff.Skip((query.PageNumber - 1) * query.PageSize)
                                .Take(query.PageSize)
                                .Adapt<List<GetAllStaffWithPaginationQueryDto>>();

            return PaginatedRespnose<GetAllStaffWithPaginationQueryDto>.Create(entities, staff.Count, query.PageNumber, query.PageSize);
        }
    }
}
