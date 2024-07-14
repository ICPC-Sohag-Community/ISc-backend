using ISc.Application.Extension;
using ISc.Application.Interfaces.Repos;
using ISc.Domain.Models;
using ISc.Domain.Models.IdentityModels;
using ISc.Shared;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISc.Application.Features.HeadOfCamps.Sessions.Query.GetAllWithPagination
{
	public record GetAllSessionsWithPaginationQuery:PaginatedRequest,IRequest<PaginatedRespnose<GetAllSessionsWithPaginationQueryDto>>;
	internal class GetAllSessionsWithPaginationQueryHandler : IRequestHandler<GetAllSessionsWithPaginationQuery, PaginatedRespnose<GetAllSessionsWithPaginationQueryDto>>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IHttpContextAccessor _contextAccessor;
		private readonly UserManager<Account> _userManager;

		public GetAllSessionsWithPaginationQueryHandler(
			UserManager<Account> userManager,
			IHttpContextAccessor contextAccessor,
			IUnitOfWork unitOfWork)
		{
			_userManager = userManager;
			_contextAccessor = contextAccessor;
			_unitOfWork = unitOfWork;
		}

		public async Task<PaginatedRespnose<GetAllSessionsWithPaginationQueryDto>> Handle(GetAllSessionsWithPaginationQuery query, CancellationToken cancellationToken)
		{
			var user = await _userManager.GetUserAsync(_contextAccessor.HttpContext!.User);
			
			if(user is null)
			{
				return await Response.FailureAsync<GetAllSessionsWithPaginationQueryDto>("Unauthorized", System.Net.HttpStatusCode.Unauthorized);
			}

			var head = await _unitOfWork.Heads.GetByIdAsync(user.Id);

			if(head is null)
			{
				return await Response.FailureAsync<GetAllSessionsWithPaginationQueryDto>("Unauthorized", System.Net.HttpStatusCode.Unauthorized);
			}

			var Sessions = _unitOfWork.Repository<Session>().Entities
									  .Where(x => x.CampId == head.CampId)
									  .OrderBy(x => x.StartDate);

			return await Sessions.ProjectToType<GetAllSessionsWithPaginationQueryDto>()
					.ToPaginatedListAsync(query.PageNumber, query.PageSize, cancellationToken);
		}
	}
}
