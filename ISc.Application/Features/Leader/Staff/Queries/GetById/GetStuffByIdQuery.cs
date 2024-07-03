using ISc.Application.Features.Leader.Trainees.Queries.GetById;
using ISc.Application.Features.SystemRoles.Queries.GetUserRoles;
using ISc.Application.Interfaces;
using ISc.Application.Interfaces.Repos;
using ISc.Domain.Models;
using ISc.Domain.Models.IdentityModels;
using ISc.Shared;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ISc.Application.Features.Leader.Staff.Queries.GetById
{
	public record GetStuffByIdQuery : IRequest<Response>
	{
        public string Id { get; set; }

        public GetStuffByIdQuery(string id)
        {
            Id = id;
        }
    }
	internal class GetStuffByIdQueryHandler : IRequestHandler<GetStuffByIdQuery, Response>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly UserManager<Account> _userManager;
		private readonly IMediaServices _mediaServices;
		private readonly IMediator _mediator;

		public GetStuffByIdQueryHandler(
			IUnitOfWork unitOfWork,
			UserManager<Account> userManager,
			IMediaServices mediaServices,
			IMediator mediator)
		{
			_unitOfWork = unitOfWork;
			_userManager = userManager;
			_mediaServices = mediaServices;
			_mediator = mediator;
		}

		public async Task<Response> Handle(GetStuffByIdQuery query, CancellationToken cancellationToken)
		{
			var account = await _userManager.FindByIdAsync(query.Id);
			if (account == null)
			{
				return await Response.FailureAsync("no data!", HttpStatusCode.NotFound);
			}
			var stuff = account.Adapt<GetTraineeByIdQueryDto>();

			stuff.FullName = $"{account.FirstName}' '{account.MiddleName}' '{account.LastName}";

			stuff.PhotoUrl = _mediaServices.GetUrl(stuff.PhotoUrl);

			stuff.UserRoles = await _mediator.Send(new GetUserRolesQuery(account));

			return await Response.SuccessAsync(stuff);

		}
	}
}
