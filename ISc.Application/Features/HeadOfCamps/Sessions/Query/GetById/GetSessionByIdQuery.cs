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
		private readonly HttpContextAccessor _contextAccessor;
		private readonly UserManager<Account> _userManager;
		private readonly IMapper _mapper;

		public GetSessionByIdQueryHandler(
			UserManager<Account> userManager,
			HttpContextAccessor contextAccessor,
			IUnitOfWork unitOfWork,
			IMapper mapper)
		{
			_userManager = userManager;
			_contextAccessor = contextAccessor;
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<Response> Handle(GetSessionByIdQuery query, CancellationToken cancellationToken)
		{
			var user = await _userManager.GetUserAsync(_contextAccessor.HttpContext.User!);

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

			if(entity.CampId!=head.CampId)
			{
				return await Response.FailureAsync("Unauthorized", System.Net.HttpStatusCode.Unauthorized);
			}

			var session = new GetSessionByIdQueryDto()
			{
				Id = entity.Id,
				Topic=entity.Topic,
				LocationLink=entity.LocationLink,
				LocationName=entity.LocationName,
				InstructorName=entity.InstructorName,
				StartDate=entity.StartDate,
				EndDate=entity.EndDate
			};

			return await Response.SuccessAsync(session);
		}
	}
}
