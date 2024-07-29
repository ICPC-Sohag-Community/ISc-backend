using ISc.Application.Interfaces;
using ISc.Application.Interfaces.Repos;
using ISc.Domain.Models.IdentityModels;
using ISc.Shared;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ISc.Application.Features.Mentors.TraineesInfo.Queries.GetTraineesInfo
{
    public record GetTraineesInfoQuery : IRequest<Response>
    {
        public int CampId { get; set; }

        public GetTraineesInfoQuery(int campId)
        {
            CampId = campId;
        }
    }

    internal class GetTraineesInfoQueryHandler : IRequestHandler<GetTraineesInfoQuery, Response>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly UserManager<Account> _userManager;
        private readonly IMediaServices _mediaServices;

        public GetTraineesInfoQueryHandler(
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

        public async Task<Response> Handle(GetTraineesInfoQuery query, CancellationToken cancellationToken)
        {
            var mentor = await _userManager.GetUserAsync(_contextAccessor.HttpContext!.User);

            if (mentor is null)
            {
                return await Response.FailureAsync("Unauthorized.", System.Net.HttpStatusCode.Unauthorized);
            }

            var trainees = await _unitOfWork.Trainees.Entities
                .Where(x => x.MentorId == mentor.Id && x.CampId == query.CampId)
                .ProjectToType<GetTraineesInfoQueryDto>()
                .ToListAsync();

            foreach (var trainee in trainees)
            {
                trainee.PhotoUrl = _mediaServices.GetUrl(trainee.PhotoUrl);
            }

            return await Response.SuccessAsync(trainees);
        }
    }
}
