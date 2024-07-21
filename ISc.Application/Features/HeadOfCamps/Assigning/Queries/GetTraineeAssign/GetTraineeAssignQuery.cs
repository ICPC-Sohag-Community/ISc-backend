using ISc.Application.Extension;
using ISc.Application.Interfaces;
using ISc.Application.Interfaces.Repos;
using ISc.Domain.Models.IdentityModels;
using ISc.Shared;
using Mapster;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace ISc.Application.Features.HeadOfCamps.Assigning.Queries.GetTraineeAssign
{ 
    public record GetTraineeAssignQuery : IRequest<Response>
    {
        public string? KeyWord { get; set; }
        public SortBy? SortBy { get; set; }
    }

    internal class GetTraineeAssignQueryHandler : IRequestHandler<GetTraineeAssignQuery, Response>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<Account> _userManager;
        private readonly IHttpContextAccessor _context;
        private readonly IMediaServices _mediaServices;

        public GetTraineeAssignQueryHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IHttpContextAccessor httpContext,
            UserManager<Account> userManager,
            IMediaServices mediaServices)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _context = httpContext;
            _userManager = userManager;
            _mediaServices = mediaServices;
        }

        public async Task<Response> Handle(GetTraineeAssignQuery query, CancellationToken cancellationToken)
        {
            var user = await _userManager.GetUserAsync(_context.HttpContext!.User);

            if (user is null)
            {
                 return  await Response.FailureAsync("Unauthorized", System.Net.HttpStatusCode.Unauthorized);
            }

            var head = await _unitOfWork.Heads.GetByIdAsync(user.Id);

            if (head is null)
            {
                return Response.FailureAsync("not allowed", System.Net.HttpStatusCode.MethodNotAllowed)
                           .Result.Adapt<PaginatedRespnose<GetTraineeAssignQueryDto>>();
            }

            var entities = _unitOfWork.Trainees.Entities.Where(x => x.MentorId == null && x.CampId == head.CampId);

            if (query.SortBy != null)
            {
                switch (query.SortBy)
                {
                    case SortBy.College:
                        entities = entities.OrderBy(x => x.Account.College);
                        break;
                    case SortBy.Grade:
                        entities = entities.OrderBy(x => x.Account.Grade);
                        break;
                    case SortBy.Gender:
                        entities = entities.OrderBy(x => x.Account.Gender);
                        break;
                }
            }

            if (!query.KeyWord.IsNullOrEmpty())
            {
                entities = entities.Where(x =>
                (x.Account.FirstName + x.Account.MiddleName + x.Account.LastName).Trim().ToLower().StartsWith(query.KeyWord!.Trim().ToLower())
                || x.Account.CodeForceHandle.Trim().ToLower().StartsWith(query.KeyWord.Trim().ToLower()));
            }

            var trainees = await entities.ToListAsync();

            foreach(var trainee in trainees)
            {
                trainee.Account.PhotoUrl = _mediaServices.GetUrl(trainee.Account.PhotoUrl);
            }

            return await Response.SuccessAsync(trainees.Adapt<List<GetTraineeAssignQueryDto>>());

        }
    }
}
