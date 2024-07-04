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
	public record GetStaffByIdQuery : IRequest<Response>
	{
        public string Id { get; set; }

        public GetStaffByIdQuery(string id)
        {
            Id = id;
        }
    }
	internal class GetStuffByIdQueryHandler : IRequestHandler<GetStaffByIdQuery, Response>
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

		public async Task<Response> Handle(GetStaffByIdQuery query, CancellationToken cancellationToken)
		{
			var account = await _userManager.FindByIdAsync(query.Id);

			if (account == null)
			{
				return await Response.FailureAsync("Account not found.", HttpStatusCode.NotFound);
			}

			var staff = account.Adapt<GetStaffByIdQueryDto>();

			staff.PhotoUrl = _mediaServices.GetUrl(staff.PhotoUrl);

			staff.UserRoles = await _mediator.Send(new GetUserRolesQuery(account));

			return await Response.SuccessAsync(staff);
		}
	}
}
