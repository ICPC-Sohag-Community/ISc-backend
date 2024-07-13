using FluentValidation;
using ISc.Application.Features.HeadOfCamps.Sheets.Commands.Update;
using ISc.Application.Interfaces.Repos;
using ISc.Domain.Models;
using ISc.Domain.Models.IdentityModels;
using ISc.Shared;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISc.Application.Features.HeadOfCamps.Materials.Commands.Update
{
	public record UpdateMateriaCommand:IRequest<Response>
	{
		public int Id { get; set; }
		public int SheetId { get; set; }
		public string Title { get; set; }
		public string Link { get; set; }
		public int MaterialOrder { get; set; }


	}
	internal class UpdateMaterialCommandHandler : IRequestHandler<UpdateMateriaCommand, Response>
	{
		private readonly IUnitOfWork _unitOfWork;

		private readonly IHttpContextAccessor _contextAccessor;

		private readonly UserManager<Account> _userManager;

		private readonly IMapper _mapper;

		private readonly IValidator<UpdateMateriaCommand> _validator;

		public UpdateMaterialCommandHandler(
			IUnitOfWork unitOfWork,
			IHttpContextAccessor contextAccessor,
			UserManager<Account> userManager,
			IMapper mapper,
			IValidator<UpdateMateriaCommand> validator)
		{
			_unitOfWork = unitOfWork;
			_contextAccessor = contextAccessor;
			_userManager = userManager;
			_mapper = mapper;
			_validator = validator;
		}


		public async Task<Response> Handle(UpdateMateriaCommand command, CancellationToken cancellationToken)
		{
			var validationResult = await _validator.ValidateAsync(command);

			if (!validationResult.IsValid)
			{
				return await Response.ValidationFailureAsync(validationResult.Errors, System.Net.HttpStatusCode.UnprocessableEntity);
			}

			var user = await _userManager.GetUserAsync(_contextAccessor.HttpContext.User!);

			if(user is null)
			{
				return await Response.FailureAsync("Unauthorized", System.Net.HttpStatusCode.Unauthorized);
			}

			var head = await _unitOfWork.Heads.GetByIdAsync(user.Id);

			if(head is null)
			{
				return await Response.FailureAsync("Unauthorized", System.Net.HttpStatusCode.Unauthorized);
			}

			var sheet = await _unitOfWork.Repository<Sheet>().Entities
					   .SingleOrDefaultAsync(x => x.CampId == head.CampId && x.Id == command.SheetId);

			if (sheet is null)
			{
				return await Response.FailureAsync("Sheet not found.", System.Net.HttpStatusCode.NotFound);
			}

			var materials = sheet.Materials.ToHashSet();

			var materiale = materials.FirstOrDefault(x => x.Id == command.Id);

			if(materiale is null)
			{
				return await Response.FailureAsync("material not found", System.Net.HttpStatusCode.NotFound);
			}

			foreach (var material in materials)
			{
				if(material.Id!=command.Id && material.Title==command.Title|| material.Link==command.Link)
				{
					return await Response.FailureAsync("Name or link is used before", System.Net.HttpStatusCode.BadRequest);
				}
			}
			_mapper.Map(command, materiale);

			await _unitOfWork.Repository<Material>().UpdateAsync(materiale);
			await _unitOfWork.SaveAsync();

			return await Response.SuccessAsync(materiale.Id, "Material updated");

		}
	}
}
