using ISc.Application.Extension;
using ISc.Application.Interfaces.Repos;
using ISc.Domain.Models;
using ISc.Domain.Models.IdentityModels;
using ISc.Shared;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace ISc.Application.Features.HeadOfCamps.Sheets.Queries.GetAllWithPagination
{
    public record GetAllSheetsWithPaginationQuery : PaginatedRequest, IRequest<PaginatedRespnose<GetAllSheetsWithPaginationQueryDto>>
    {

    }
    internal class GetAllSheetsWithPaginationQueryHandler : IRequestHandler<GetAllSheetsWithPaginationQuery, PaginatedRespnose<GetAllSheetsWithPaginationQueryDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly UserManager<Account> _userManager;
        public GetAllSheetsWithPaginationQueryHandler(
            IUnitOfWork unitOfWork,
            IHttpContextAccessor contextAccessor,
            UserManager<Account> userManager)
        {
            _unitOfWork = unitOfWork;
            _contextAccessor = contextAccessor;
            _userManager = userManager;
        }

        public async Task<PaginatedRespnose<GetAllSheetsWithPaginationQueryDto>> Handle(GetAllSheetsWithPaginationQuery query, CancellationToken cancellationToken)
        {
            var user = await _userManager.GetUserAsync(_contextAccessor.HttpContext!.User);

            if (user == null)
            {
                return await Response.FailureAsync<GetAllSheetsWithPaginationQueryDto>("Unauthorized.", System.Net.HttpStatusCode.Unauthorized);
            }

            var head = await _unitOfWork.Heads.GetByIdAsync(user.Id);

            if (head is null)
            {
                return await Response.FailureAsync<GetAllSheetsWithPaginationQueryDto>("Unauthorized.", System.Net.HttpStatusCode.Unauthorized);
            }

            var sheets = _unitOfWork.Repository<Sheet>().Entities.Where(x => x.CampId == head.CampId);


            return await sheets.ProjectToType<GetAllSheetsWithPaginationQueryDto>()
                               .ToPaginatedListAsync(query.PageNumber, query.PageSize, cancellationToken);

        }
    }
}
