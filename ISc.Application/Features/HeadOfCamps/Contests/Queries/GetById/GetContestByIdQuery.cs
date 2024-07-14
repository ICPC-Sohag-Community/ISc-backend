using ISc.Application.Interfaces.Repos;
using ISc.Domain.Models;
using ISc.Domain.Models.IdentityModels;
using ISc.Shared;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISc.Application.Features.HeadOfCamps.Contests.Queries.GetById
{
	public record GetContestByIdQuery:IRequest<Response>
	{
        public int Id { get; set; }

		public GetContestByIdQuery(int id)
		{
			Id = id;
		}
	}
	internal class GetContestByIdQueryHandler : IRequestHandler<GetContestByIdQuery, Response>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IHttpContextAccessor _contextAccessor;
		private readonly UserManager<Account> _userManager;

		public GetContestByIdQueryHandler(
			UserManager<Account> userManager,
			IHttpContextAccessor contextAccessor,
			IUnitOfWork unitOfWork)
		{
			_userManager = userManager;
			_contextAccessor = contextAccessor;
			_unitOfWork = unitOfWork;
		}

		public async Task<Response> Handle(GetContestByIdQuery query, CancellationToken cancellationToken)
		{
			var user =await _userManager.GetUserAsync(_contextAccessor.HttpContext!.User);
			
			if(user is null)
			{
				return await Response.FailureAsync("Unauthorized", System.Net.HttpStatusCode.Unauthorized);
			}
			var head =await _unitOfWork.Heads.GetByIdAsync(user.Id);
			
			if(head is null)
			{
				return await Response.FailureAsync("Unauthorized", System.Net.HttpStatusCode.Unauthorized);
			}

			var entity =await _unitOfWork.Repository<Contest>().GetByIdAsync(query.Id);
			
			if(entity is null)
			{
				return await Response.FailureAsync("NotFound.", System.Net.HttpStatusCode.NotFound);

			}

			if(entity.CampId!=head.CampId)
			{
				return await Response.FailureAsync("Unauthorized", System.Net.HttpStatusCode.Unauthorized);
			}

			var contest = entity.Adapt<GetContestByIdQueryDto>();

			return await Response.SuccessAsync(contest);
		}
	}
}
