using ISc.Application.Extension;
using ISc.Application.Interfaces.Repos;
using ISc.Domain.Models;
using ISc.Shared;
using Mapster;
using MediatR;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ISc.Application.Features.Leader.Archives.Queries.GetAllStaffsArchiveWithPagination.GetAllStaffsArchiveWithPaginationQueryDto;

namespace ISc.Application.Features.Leader.Archives.Queries.GetAllStaffsArchiveWithPagination
{
	public record GetAllStaffsArchiveWithPaginationQuery:PaginatedRequest,IRequest<PaginatedRespnose<GetAllStaffsArchiveWithPaginationQueryDto>>
	{
        public GetAllTraineesArchiveWithPaginationQueryDtoColumn? SortBy { get; set; }
    }
	internal class GetAllStaffsArchiveWithPaginationQueryHandler : IRequestHandler<GetAllStaffsArchiveWithPaginationQuery, PaginatedRespnose<GetAllStaffsArchiveWithPaginationQueryDto>>
	{
		private readonly IUnitOfWork _unitOfWork;

		public GetAllStaffsArchiveWithPaginationQueryHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<PaginatedRespnose<GetAllStaffsArchiveWithPaginationQueryDto>> Handle(GetAllStaffsArchiveWithPaginationQuery query, CancellationToken cancellationToken)
		{
			var archives = _unitOfWork.Repository<StaffArchive>().Entities;

			if (!query.KeyWord.IsNullOrEmpty())
			{
				archives = archives.Where(x => (x.FirstName + x.MiddleName + x.LastName).Trim().ToLower().StartsWith(query.KeyWord.Trim().ToLower())
											   || x.CodeForceHandle.StartsWith(query.KeyWord));
				if (query.SortBy is not null)
				{
					if (query.SortBy == GetAllTraineesArchiveWithPaginationQueryDtoColumn.Gender)
					{
						archives = archives.OrderBy(x => x.Gender);
					}
					else if (query.SortBy == GetAllTraineesArchiveWithPaginationQueryDtoColumn.College)
					{
						archives = archives.OrderBy(x => x.College);
					}
					else if (query.SortBy == GetAllTraineesArchiveWithPaginationQueryDtoColumn.Grade)
					{
						archives = archives.OrderBy(x => x.Grade);
					}
				}
			}
			return await archives.ProjectToType<GetAllStaffsArchiveWithPaginationQueryDto>()
					.ToPaginatedListAsync(query.PageNumber,query.PageSize,cancellationToken);
		}
	}
}
