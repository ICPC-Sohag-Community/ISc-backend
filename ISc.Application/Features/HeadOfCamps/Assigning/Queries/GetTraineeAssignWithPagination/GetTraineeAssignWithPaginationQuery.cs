using ISc.Application.Extension;
using ISc.Application.Interfaces.Repos;
using ISc.Domain.Models.IdentityModels;
using ISc.Shared;
using Mapster;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace ISc.Application.Features.HeadOfCamps.Assigning.Queries.GetTraineeAssignWithPagination
{
    public record GetTraineeAssignWithPaginationQuery : PaginatedRequest, IRequest<PaginatedRespnose<GetTraineeAssignWithPaginationQueryDto>>
    {
        public SortBy? SortBy { get; set; }
    }

    internal class GetTraineeAssignWithPaginationQueryHandler : IRequestHandler<GetTraineeAssignWithPaginationQuery, PaginatedRespnose<GetTraineeAssignWithPaginationQueryDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<Account> _userManager;
        private readonly IHttpContextAccessor _context;

        public GetTraineeAssignWithPaginationQueryHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IHttpContextAccessor httpContext,
            UserManager<Account> userManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _context = httpContext;
            _userManager = userManager;
        }

        public async Task<PaginatedRespnose<GetTraineeAssignWithPaginationQueryDto>> Handle(GetTraineeAssignWithPaginationQuery query, CancellationToken cancellationToken)
        {
            var user = await _userManager.GetUserAsync(_context.HttpContext.User);

            if (user is null)
            {
                 return  Response.FailureAsync("Unauthorized", System.Net.HttpStatusCode.Unauthorized)
                           .Result.Adapt<PaginatedRespnose<GetTraineeAssignWithPaginationQueryDto>>();
            }

            var head = await _unitOfWork.Heads.GetByIdAsync(user.Id);

            if (head is null)
            {
                return Response.FailureAsync("not allowed", System.Net.HttpStatusCode.MethodNotAllowed)
                           .Result.Adapt<PaginatedRespnose<GetTraineeAssignWithPaginationQueryDto>>();
            }

            var trainees = _unitOfWork.Trainees.Entities.Where(x => x.MentorId == null && x.CampId == head.CampId);

            if (query.SortBy != null)
            {
                switch (query.SortBy)
                {
                    case SortBy.College:
                        trainees = trainees.OrderBy(x => x.Account.College);
                        break;
                    case SortBy.Grade:
                        trainees = trainees.OrderBy(x => x.Account.Grade);
                        break;
                    case SortBy.Gender:
                        trainees = trainees.OrderBy(x => x.Account.Gender);
                        break;
                }
            }

            if (!query.KeyWord.IsNullOrEmpty())
            {
                trainees = trainees.Where(x =>
                (x.Account.FirstName + x.Account.MiddleName + x.Account.LastName).Trim().ToLower().StartsWith(query.KeyWord!.Trim().ToLower())
                || x.Account.CodeForceHandle.Trim().ToLower().StartsWith(query.KeyWord.Trim().ToLower()));
            }

            return await trainees.ProjectToType<GetTraineeAssignWithPaginationQueryDto>(_mapper.Config)
                                 .ToPaginatedListAsync(query.PageNumber, query.PageSize, cancellationToken);

        }
    }
}
