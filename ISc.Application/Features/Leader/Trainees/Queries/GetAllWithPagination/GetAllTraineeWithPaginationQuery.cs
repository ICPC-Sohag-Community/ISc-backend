using ISc.Application.Extension;
using ISc.Application.Interfaces.Repos;
using ISc.Domain.Models;
using ISc.Shared;
using Mapster;
using MediatR;
using Microsoft.IdentityModel.Tokens;

namespace ISc.Application.Features.Leader.Trainees.Queries.GetAllWithPagination
{
    public record GetAllTraineeWithPaginationQuery : PaginatedRequest, IRequest<PaginatedRespnose<GetAllTraineeWithPaginationQueryDto>>
    {
        public GetAllTraineeWithPaginationQueryDtoColumn? SortBy { get; set; }
    }

    internal class GetAllTraineeWithPaginationQueryHandler : IRequestHandler<GetAllTraineeWithPaginationQuery, PaginatedRespnose<GetAllTraineeWithPaginationQueryDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllTraineeWithPaginationQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PaginatedRespnose<GetAllTraineeWithPaginationQueryDto>> Handle(GetAllTraineeWithPaginationQuery query, CancellationToken cancellationToken)
        {
            var entities = _unitOfWork.Trainees.Entities.Select(x=>x.Account);

            if (!query.KeyWord.IsNullOrEmpty())
            {
                entities = entities.Where(x => (x.FirstName+x.MiddleName+x.LastName).Trim().Replace(" ", "").ToLower().StartsWith(query.KeyWord.Trim().Replace(" ", "").ToLower())
                                        || x.PhoneNumber.StartsWith(query.KeyWord)
                                        || x.Email.Trim().ToLower().StartsWith(query.KeyWord.Trim().ToLower())
                                        || x.CodeForceHandle.ToLower().StartsWith(query.KeyWord.ToLower()));
            }

            if (query.SortBy is not null)
            {
                if(query.SortBy== GetAllTraineeWithPaginationQueryDtoColumn.Faculty)
                {
                    entities = entities.OrderBy(x => x.College);
                }
                else if(query.SortBy== GetAllTraineeWithPaginationQueryDtoColumn.Grade)
                {
                    entities = entities.OrderBy(x => x.Grade);
                }
                else if (query.SortBy == GetAllTraineeWithPaginationQueryDtoColumn.Gender)
                {
                    entities = entities.OrderBy(x => x.Gender);
                }
            }


            return await entities
                .ProjectToType<GetAllTraineeWithPaginationQueryDto>()
                .ToPaginatedListAsync(query.PageNumber, query.PageSize, cancellationToken);
        }
    }
}
