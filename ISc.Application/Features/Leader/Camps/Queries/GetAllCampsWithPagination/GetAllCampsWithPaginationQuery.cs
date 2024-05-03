using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISc.Application.Extension;
using ISc.Application.Interfaces.Repos;
using ISc.Domain.Models;
using ISc.Shared;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ISc.Application.Features.Leader.Camps.Queries.GetAllCampsWithPagination
{
    public record GetAllCampsWithPaginationQuery:PaginatedRequest,IRequest<PaginatedRespnose<GetAllCampsWithPaginationQueryDto>>;

    internal class GetAllCampsWithPaginationQueryHandler : IRequestHandler<GetAllCampsWithPaginationQuery, PaginatedRespnose<GetAllCampsWithPaginationQueryDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllCampsWithPaginationQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PaginatedRespnose<GetAllCampsWithPaginationQueryDto>> Handle(GetAllCampsWithPaginationQuery query, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Repository<Camp>().Entities
                           .ProjectToType<GetAllCampsWithPaginationQueryDto>()
                           .ToPaginatedListAsync(query.PageNumber, query.PageSize, cancellationToken);
        }
    }
}
