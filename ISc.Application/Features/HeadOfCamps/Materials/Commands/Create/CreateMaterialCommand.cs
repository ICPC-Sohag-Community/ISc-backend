using FluentValidation;
using ISc.Application.Interfaces.Repos;
using ISc.Domain.Models;
using ISc.Domain.Models.IdentityModels;
using ISc.Shared;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISc.Application.Features.HeadOfCamps.Materials.Commands.Create
{
    public record CreateMaterialCommand:IRequest<Response>
    {
        public string Title { get; set; }
        public string Link { get; set; }
        public int SheetId { get; set; }
    }

    internal class CreateMaterialCommandHandler : IRequestHandler<CreateMaterialCommand, Response>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IValidator<CreateMaterialCommand> _validator;
        private readonly UserManager<Account> _userManager;

        public CreateMaterialCommandHandler(
            IUnitOfWork unitOfWork,
            IHttpContextAccessor contextAccessor,
            UserManager<Account> userManager,
            IValidator<CreateMaterialCommand> validator)
        {
            _unitOfWork = unitOfWork;
            _contextAccessor = contextAccessor;
            _userManager = userManager;
            _validator = validator;
        }

        public async Task<Response> Handle(CreateMaterialCommand command, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(command, cancellationToken);

            if (!validationResult.IsValid)
            {
                return await Response.ValidationFailureAsync(validationResult.Errors, System.Net.HttpStatusCode.UnprocessableEntity);
            }

            var user = await _userManager.GetUserAsync(_contextAccessor.HttpContext!.User);

            if(user is null)
            {
                return await Response.FailureAsync("Unauthorized.", System.Net.HttpStatusCode.Unauthorized);
            }

            var head = await _unitOfWork.Heads.GetByIdAsync(user.Id);
            
            if(head is null)
            {
                return await Response.FailureAsync("Unauthorized.", System.Net.HttpStatusCode.Unauthorized);
            }

            var sheet = await _unitOfWork.Repository<Sheet>().GetByIdAsync(command.SheetId);

            if(sheet is null || sheet.CampId != head.CampId)
            {
                return await Response.FailureAsync("Sheet not found.", System.Net.HttpStatusCode.NotFound);
            }

            var material = command.Adapt<Material>();

            material.MaterialOrder = await _unitOfWork.Repository<Material>().Entities.Where(x=>x.SheetId==sheet.Id).CountAsync(cancellationToken) + 1;

            await _unitOfWork.Repository<Material>().AddAsync(material);
            await _unitOfWork.SaveAsync();

            return await Response.SuccessAsync(material.Id, "Material added");
        }
    }
}
