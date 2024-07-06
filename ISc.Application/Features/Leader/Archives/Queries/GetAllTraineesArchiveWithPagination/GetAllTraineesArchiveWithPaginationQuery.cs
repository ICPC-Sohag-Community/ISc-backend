using ISc.Application.Extension;
using ISc.Application.Interfaces.Repos;
using ISc.Domain.Models;
using ISc.Shared;
using Mapster;
using MediatR;
using Microsoft.IdentityModel.Tokens;

namespace ISc.Application.Features.Leader.Archives.Queries.GetAllTraineesArchiveWithPagination
{
    public record GetAllTraineesArchiveWithPaginationQuery : PaginatedRequest, IRequest<PaginatedRespnose<GetAllTraineesArchiveWithPaginationQueryDto>>
    {
        public GetAllTraineeArchivePaginationColumns? SortBy { get; set; }
    }

    internal class GetAllTraineesArchiveWithPaginationQueryHandler : IRequestHandler<GetAllTraineesArchiveWithPaginationQuery, PaginatedRespnose<GetAllTraineesArchiveWithPaginationQueryDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllTraineesArchiveWithPaginationQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PaginatedRespnose<GetAllTraineesArchiveWithPaginationQueryDto>> Handle(GetAllTraineesArchiveWithPaginationQuery query, CancellationToken cancellationToken)
        {
            var archives = _unitOfWork.Repository<TraineeArchive>().Entities;

            if (!query.KeyWord.IsNullOrEmpty())
            {
                archives = archives.Where(x => (x.FirstName + x.MiddleName + x.LastName).Trim().ToLower().StartsWith(query.KeyWord.Trim().ToLower())
                                               || x.CampName.ToLower().Trim().StartsWith(query.KeyWord.ToLower().Trim())
                                               || x.CodeForceHandle.StartsWith(query.KeyWord));
                if (query.SortBy is not null)
                {
                    if (query.SortBy == GetAllTraineeArchivePaginationColumns.Gender)
                    {
                        archives = archives.OrderBy(x => x.Gender);
                    }
                    else if (query.SortBy == GetAllTraineeArchivePaginationColumns.College)
                    {
                        archives = archives.OrderBy(x => x.College);
                    }
                    else if (query.SortBy == GetAllTraineeArchivePaginationColumns.CampName)
                    {
                        archives=archives.OrderBy(x => x.CampName);
                    }
                }
            }

            return await archives.ProjectToType<GetAllTraineesArchiveWithPaginationQueryDto>()
                .ToPaginatedListAsync(query.PageNumber, query.PageSize, cancellationToken);
        }
    }
}
