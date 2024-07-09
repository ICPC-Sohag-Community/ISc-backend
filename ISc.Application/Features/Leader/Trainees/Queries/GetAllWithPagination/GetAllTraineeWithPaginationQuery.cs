using ISc.Application.Extension;
using ISc.Application.Interfaces.Repos;
using ISc.Shared;
using MediatR;
using Microsoft.IdentityModel.Tokens;

namespace ISc.Application.Features.Leader.Trainees.Queries.GetAllWithPagination
{
    public record GetAllTraineeWithPaginationQuery : PaginatedRequest, IRequest<PaginatedRespnose<GetAllTraineeWithPaginationQueryDto>>;

    internal class GetAllTraineeWithPaginationQueryHandler : IRequestHandler<GetAllTraineeWithPaginationQuery, PaginatedRespnose<GetAllTraineeWithPaginationQueryDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllTraineeWithPaginationQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PaginatedRespnose<GetAllTraineeWithPaginationQueryDto>> Handle(GetAllTraineeWithPaginationQuery query, CancellationToken cancellationToken)
        {
            var entities = _unitOfWork.Trainees.Entities.Select(x => new GetAllTraineeWithPaginationQueryDto()
            {
                Id = x.Id,
                CodeforceHandle = x.Account.CodeForceHandle,
                Email = x.Account.Email,
                FullName = x.Account.FirstName + ' ' + x.Account.MiddleName + ' ' + x.Account.LastName,
                PhoneNumber = x.Account.PhoneNumber
            });

            if (!query.KeyWord.IsNullOrEmpty())
            {
                entities = entities.Where(x => x.FullName.Trim().ToLower().StartsWith(query.KeyWord.Trim().ToLower())
                                        || x.PhoneNumber.StartsWith(query.KeyWord)
                                        || x.Email.Trim().ToLower().StartsWith(query.KeyWord.Trim().ToLower())
                                        || x.CodeforceHandle.Trim().ToLower().StartsWith(query.KeyWord.Trim().ToLower()));
            }

            return await entities.ToPaginatedListAsync(query.PageNumber, query.PageSize, cancellationToken);
        }
    }
}
