using FluentValidation;
using ISc.Application.Features.HeadOfCamps.Sheets.Commands.Create;
using ISc.Application.Interfaces.Repos;
using ISc.Domain.Models;
using ISc.Domain.Models.IdentityModels;
using ISc.Shared;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISc.Application.Features.HeadOfCamps.Sessions.Commands.Create
{
	public record CreateSessionCommand:IRequest<Response>
	{
		public string Topic { get; set; }
		public string InstructorName { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public string LocationName { get; set; }
		public string LocationLink { get; set; }
	}
	internal class CreateSessionCommandHandler : IRequestHandler<CreateSessionCommand, Response>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IHttpContextAccessor _contextAccessor;
		private readonly UserManager<Account> _userManager;
		private readonly IValidator<CreateSessionCommand> _validator;

		public CreateSessionCommandHandler(UserManager<Account> userManager, IUnitOfWork unitOfWork, IHttpContextAccessor contextAccessor, IValidator<CreateSessionCommand> validator)
		{
			_userManager = userManager;
			_unitOfWork = unitOfWork;
			_contextAccessor = contextAccessor;
			_validator = validator;
		}

		public async Task<Response> Handle(CreateSessionCommand command, CancellationToken cancellationToken)
		{
			var validationResult = await _validator.ValidateAsync(command);

			if (!validationResult.IsValid)
			{
				return await Response.ValidationFailureAsync(validationResult.Errors, System.Net.HttpStatusCode.UnprocessableEntity);
			}

			var user =await _userManager.GetUserAsync(_contextAccessor.HttpContext!.User);
			
			if(user is null)
			{
				return await Response.FailureAsync("Unauthorized", System.Net.HttpStatusCode.Unauthorized);
			}

			var head = await _unitOfWork.Heads.GetByIdAsync(user.Id);

			if(head is null)
			{
				return await Response.FailureAsync("Unauthorized", System.Net.HttpStatusCode.Unauthorized);
			}

			if(await _unitOfWork.Repository<Session>().Entities
				.AnyAsync(x=> x.Topic==command.Topic && x.StartDate==command.StartDate && x.CampId==head.CampId))
			{
				return await Response.FailureAsync("Seesion already exist ..!");
			}
			var session = command.Adapt<Session>();
			session.CampId = head.CampId;

			await _unitOfWork.Repository<Session>().AddAsync(session);
			await _unitOfWork.SaveAsync();

			return await Response.SuccessAsync(session.Id, "Session added Successfully.");

		}
	}
}
