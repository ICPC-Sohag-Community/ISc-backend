using ISc.Application.Interfaces.Repos;
using ISc.Domain.Comman.Constant;
using ISc.Domain.Models;
using ISc.Domain.Models.IdentityModels;
using ISc.Shared;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ISc.Application.Features.SystemRoles.Queries.GetAvailableRoles
{
    public record GetAvailableRolesQuery : IRequest<Response>
    {
        public GetAvailableRolesQuery(string userId)
        {
            UserId = userId;
        }

        public string UserId { get; set; }
    }

    internal class GetAvailableRolesQueryHandler : IRequestHandler<GetAvailableRolesQuery, Response>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<Account> _userManager;
        private readonly IHttpContextAccessor _httpContext;

        public GetAvailableRolesQueryHandler(
            IUnitOfWork unitOfWork,
            IHttpContextAccessor httpContext,
            RoleManager<IdentityRole> roleManager,
            UserManager<Account> userManager)
        {
            _unitOfWork = unitOfWork;
            _httpContext = httpContext;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<Response> Handle(GetAvailableRolesQuery query, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(query.UserId);

            if (user == null)
            {
                return await Response.FailureAsync("User not found", System.Net.HttpStatusCode.NotFound);
            }

            var userRole = await _userManager.GetRolesAsync(user);

            var availableRoles = await _roleManager.Roles.Where(x => x.Name != Roles.Trainee
                                                  && x.Name != Roles.Head_Of_Camp
                                                  && x.Name != Roles.Mentor
                                                  && !userRole.Contains(x.Name!))
                                            .Select(x => new GetAvailableRolesQueryDto()
                                            {
                                                Role = x.Name!,
                                            }).ToListAsync();

            var mentorRoles = await GetAvailableMentorCamps(user);
            var traineeRole = await GetAvailableTraineeCamps(user);
            var headRole = await GetAvailableHeadCamps(user);

            if (mentorRoles is not null) availableRoles.Add(mentorRoles);
            if (traineeRole is not null) availableRoles.Add(traineeRole);
            if (headRole is not null) availableRoles.Add(headRole);

            return await Response.SuccessAsync(availableRoles);
        }

        private async Task<GetAvailableRolesQueryDto> GetAvailableMentorCamps(Account user)
        {
            var userCamps = await _unitOfWork.Repository<MentorsOfCamp>().Entities
                            .Where(x => x.MentorId == user.Id)
                            .Select(x => x.CampId).ToListAsync();

            return new GetAvailableRolesQueryDto()
            {
                Role = Roles.Mentor,
                AvailableCamps = await _unitOfWork.Repository<Camp>().Entities
                                .Where(x => !userCamps.Contains(x.Id))
                                .Select(x => new GetAvailableRolesDependecyDto()
                                {
                                    CampId = x.Id,
                                    CampName = x.Name
                                }).ToListAsync()
            };
        }

        private async Task<GetAvailableRolesQueryDto> GetAvailableHeadCamps(Account user)
        {
            var campIdOfHead = _unitOfWork.Heads.Entities.SingleAsync(x => x.Id == user.Id).Result.CampId;

            return new GetAvailableRolesQueryDto()
            {
                Role = Roles.Head_Of_Camp,
                AvailableCamps = await _unitOfWork.Repository<Camp>().Entities
                                .Where(x => x.Id != campIdOfHead)
                                .Select(x => new GetAvailableRolesDependecyDto()
                                {
                                    CampName = x.Name,
                                    CampId = x.Id,
                                }).ToListAsync()
            };
        }

        private async Task<GetAvailableRolesQueryDto> GetAvailableTraineeCamps(Account user)
        {
            var campId = _unitOfWork.Trainees.GetByIdAsync(user.Id).Result!.CampId;

            return new GetAvailableRolesQueryDto()
            {
                Role = Roles.Trainee,
                AvailableCamps = await _unitOfWork.Repository<Camp>().Entities
                                .Where(x => x.Id != campId)
                                .Select(x => new GetAvailableRolesDependecyDto()
                                {
                                    CampName = x.Name,
                                    CampId = x.Id,
                                }).ToListAsync()
            };
        }
    }
}
