using ISc.Application.Interfaces.Repos;
using ISc.Domain.Models.IdentityModels;
using ISc.Shared;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace ISc.Application.Features.HeadOfCamps.Standing.Queries.GetStanding
{
    public record GetStandingQuery : IRequest<Response>;

    internal class GetStandingQueryHandler : IRequestHandler<GetStandingQuery, Response>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<Account> _userManager;
        private readonly IHttpContextAccessor _contextAccessor;
        public GetStandingQueryHandler(
            IUnitOfWork unitOfWork,
            UserManager<Account> userManager,
            IHttpContextAccessor contextAccessor)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _contextAccessor = contextAccessor;
        }

        public async Task<Response> Handle(GetStandingQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.GetUserAsync(_contextAccessor.HttpContext!.User);

            var head = await _unitOfWork.Heads.GetByIdAsync(user.Id);

            if (head is null)
            {
                return await Response.FailureAsync("Unauthorized.", System.Net.HttpStatusCode.Unauthorized);
            }

            var entities = await _unitOfWork.GetStandingAsync(head.CampId);

            var standing = entities.Adapt<List<GetStandingQueryDto>>();

            return await Response.SuccessAsync(standing);
        }
    }
}
