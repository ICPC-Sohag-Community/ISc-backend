using ISc.Application.Interfaces.Repos;
using ISc.Domain.Comman.Constant;
using ISc.Domain.Models;
using ISc.Domain.Models.IdentityModels;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ISc.Application.Features.SystemRoles.Queries.GetUserRoles
{
    public record GetUserRolesQuery : IRequest<List<GetUserRolesQueryDto>>
    {
        public Account User { get; set; }

        public GetUserRolesQuery(Account user)
        {
            User = user;
        }
    }

    internal class GetUserRolesQueryHandler : IRequestHandler<GetUserRolesQuery, List<GetUserRolesQueryDto>>
    {
        private readonly UserManager<Account> _userManager;
        private readonly IUnitOfWork _unitOfWork;

        public GetUserRolesQueryHandler(
            UserManager<Account> userManager,
            IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
        }


        public async Task<List<GetUserRolesQueryDto>> Handle(GetUserRolesQuery query, CancellationToken cancellationToken)
        {
            var roles = await _userManager.GetRolesAsync(query.User);

            var userRoles = roles.Where(x => x != Roles.Trainee
                                        && x != Roles.Head_Of_Camp
                                        && x != Roles.Mentor)
                                 .Select(x => new GetUserRolesQueryDto()
                                 {
                                     Role = x
                                 }).ToList();

            if (roles.Contains(Roles.Mentor))
            {
                var mentorsDetails = GetMentorDetails(query.User);
                if (mentorsDetails != null)
                {
                    userRoles.AddRange(await mentorsDetails);
                }
            }

            if (roles.Contains(Roles.Trainee))
            {
                var traineeDetail = GetTraineeDetails(query.User);
                if (traineeDetail != null)
                {
                    userRoles.Add(traineeDetail);

                }
            }

            if (roles.Contains(Roles.Head_Of_Camp))
            {
                var headDetail = GetHocDetails(query.User);
                if (headDetail != null)
                {
                    userRoles.Add(headDetail);
                }
            }

            return userRoles;
        }
        private async Task<List<GetUserRolesQueryDto>> GetMentorDetails(Account user)
        {
            return await _unitOfWork.Repository<MentorsOfCamp>().Entities
                            .Where(x => x.MentorId == user.Id)
                            .Select(x => new GetUserRolesQueryDto()
                            {
                                Role = Roles.Mentor,
                                CampId = x.CampId,
                                CampName = x.Camp.Name
                            }).ToListAsync();
        }
        private GetUserRolesQueryDto GetTraineeDetails(Account user)
        {
            var detail = _unitOfWork.Trainees.Entities
                        .SingleAsync(x => x.Id == user.Id)
                        .Result;

            return new GetUserRolesQueryDto()
            {
                Role = Roles.Trainee,
                CampId = detail.CampId,
                CampName = detail.Camp.Name
            };
        }
        private GetUserRolesQueryDto GetHocDetails(Account user)
        {
            var detail = _unitOfWork.Heads.Entities
                        .SingleAsync(x => x.Id == user.Id)
                        .Result;

            return new GetUserRolesQueryDto()
            {
                Role = Roles.Head_Of_Camp,
                CampId = detail.CampId,
                CampName = detail.Camp.Name
            };
        }
    }
}
