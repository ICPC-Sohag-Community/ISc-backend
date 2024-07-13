using ISc.Application.Interfaces.Repos;
using ISc.Domain.Models;
using ISc.Domain.Models.IdentityModels;
using ISc.Shared;
using Mapster;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISc.Application.Features.HeadOfCamps.Sessions.Query.GetById
{
	public record GetSessionByIdQuery:IRequest<Response>
	{
        public int Id { get; set; }

		public GetSessionByIdQuery(int id)
		{
			Id = id;
		}
	}
	internal class GetSessionByIdQueryHandler: IRequestHandler<GetSessionByIdQuery, Response>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IHttpContextAccessor _contextAccessor;
		private readonly UserManager<Account> _userManager;

		public GetSessionByIdQueryHandler(
			UserManager<Account> userManager,
			IHttpContextAccessor contextAccessor,
			IUnitOfWork unitOfWork)
		{
			_userManager = userManager;
			_contextAccessor = contextAccessor;
			_unitOfWork = unitOfWork;
		}

		public async Task<Response> Handle(GetSessionByIdQuery query, CancellationToken cancellationToken)
		{
			var user = await _userManager.GetUserAsync(_contextAccessor.HttpContext!.User);

			if(user is null)
			{
				return await Response.FailureAsync("Unauthorized", System.Net.HttpStatusCode.Unauthorized);
			}

			var head =await _unitOfWork.Heads.GetByIdAsync(user.Id);

			if(head is null)
			{
				return await Response.FailureAsync("Unauthorized", System.Net.HttpStatusCode.Unauthorized);
			}

			var entity = await _unitOfWork.Repository<Session>().GetByIdAsync(query.Id);

			if(entity is null)
			{
				return await Response.FailureAsync("NotFound", System.Net.HttpStatusCode.NotFound);
			}

			//if(entity.CampId!=head.CampId)
			//{
			//	return await Response.FailureAsync("Unauthorized", System.Net.HttpStatusCode.Unauthorized);
			//}

			var session = entity.Adapt<GetSessionByIdQueryDto>();

			return await Response.SuccessAsync(session);
		}
	}
}
