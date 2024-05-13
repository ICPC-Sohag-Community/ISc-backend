using System.Net;
using ISc.Application.Interfaces.Repos;
using ISc.Domain.Comman.Constant;
using ISc.Domain.Models.IdentityModels;
using ISc.Shared;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ISc.Application.Features.Leader.Trainees.Queries.GetById
{
    public record GetTraineeByIdQuery : IRequest<Response>
    {
        public string Id { get; set; }

        public GetTraineeByIdQuery(string id)
        {
            Id = id;
        }
    }

    internal class GetTraineeByIdQueryHandler : IRequestHandler<GetTraineeByIdQuery, Response>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<Account> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public GetTraineeByIdQueryHandler(
            IUnitOfWork unitOfWork,
            UserManager<Account> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<Response> Handle(GetTraineeByIdQuery query, CancellationToken cancellationToken)
        {
            var entity = await _unitOfWork.Trainees.GetByIdAsync(query.Id);

            if (entity == null)
            {
                return await Response.FailureAsync("Fail to get data for this trainee.", HttpStatusCode.NotFound);
            }

            var account = entity.Account;
            var trainee = account.Adapt<GetTraineeByIdQueryDto>();
            trainee.FullName = account.FirstName + ' ' + account.MiddleName + ' ' + account.LastName;

            var inRoles = await _userManager.GetRolesAsync(entity.Account);
            var outRoles = _roleManager.Roles.Select(x => x.Name).ToListAsync().Result.Except(inRoles);

            return await Response.SuccessAsync(trainee);
        }
    }
}
