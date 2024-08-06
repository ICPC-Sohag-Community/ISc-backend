using ISc.Application.Interfaces;
using ISc.Application.Interfaces.Repos;
using ISc.Domain.Models.IdentityModels;
using ISc.Shared;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Net;

namespace ISc.Application.Features.Trainees.Heads.Queries.GetHeads
{
    public record GetHeadInfoQuery : IRequest<Response>;

    internal class GetHeadInfoQueryHandler : IRequestHandler<GetHeadInfoQuery, Response>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly UserManager<Account> _userManager;
        private readonly IMediaServices _mediaServices;

        public GetHeadInfoQueryHandler(
            IUnitOfWork unitOfWork,
            IHttpContextAccessor contextAccessor,
            UserManager<Account> userManager,
            IMediaServices mediaServices)
        {
            _unitOfWork = unitOfWork;
            _contextAccessor = contextAccessor;
            _userManager = userManager;
            _mediaServices = mediaServices;
        }

        public async Task<Response> Handle(GetHeadInfoQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.GetUserAsync(_contextAccessor.HttpContext!.User);

            if (user is null)
            {
                return await Response.FailureAsync("Unauthorized.", HttpStatusCode.Unauthorized);
            }

            var trainee = await _unitOfWork.Trainees.GetByIdAsync(user.Id);

            if (trainee == null)
            {
                return await Response.FailureAsync("Unauthorized.", HttpStatusCode.Unauthorized);
            }

            var heads = await _unitOfWork.Heads.Entities
                    .Where(x => x.CampId == trainee.CampId && x.Account.Gender == trainee.Account.Gender)
                    .Select(x => x.Account)
                    .ProjectToType<GetHeadInfoQueryDto>()
                    .ToListAsync();

            if (heads.IsNullOrEmpty())
            {
                heads = await _unitOfWork.Heads.Entities
                    .Where(x => x.CampId == trainee.CampId)
                    .Select(x => x.Account)
                    .ProjectToType<GetHeadInfoQueryDto>()
                    .ToListAsync();
            }

            foreach (var head in heads)
            {
                head.PhotoUrl = _mediaServices.GetUrl(head.PhotoUrl);
            }

            return await Response.SuccessAsync(heads);
        }
    }
}
