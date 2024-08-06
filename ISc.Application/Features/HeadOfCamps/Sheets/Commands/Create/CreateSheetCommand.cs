using FluentValidation;
using ISc.Application.Interfaces.Repos;
using ISc.Domain.Comman.Enums;
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
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ISc.Application.Features.HeadOfCamps.Sheets.Commands.Create
{
    public record CreateSheetCommand:IRequest<Response>
    {
        public string Name { get; set; }
        public string SheetLink { get; set; }
        public int MinimumPassingPrecent { get; set; }
        public Community Community { get; set; }
        public int ProblemCount { get; set; }
        public string SheetCodefroceId { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public SheetStatus Status { get; set; }
    }

    internal class CreateSheetCommandHandler : IRequestHandler<CreateSheetCommand, Response>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly UserManager<Account> _userManager;
        private readonly IValidator<CreateSheetCommand> _validator;

        public CreateSheetCommandHandler(
            IUnitOfWork unitOfWork,
            IHttpContextAccessor contextAccessor,
            UserManager<Account> userManager,
            IValidator<CreateSheetCommand> validator)
        {
            _unitOfWork = unitOfWork;
            _contextAccessor = contextAccessor;
            _userManager = userManager;
            _validator = validator;
        }

        public async Task<Response> Handle(CreateSheetCommand command, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(command);

            if (!validationResult.IsValid)
            {
                return await Response.ValidationFailureAsync(validationResult.Errors, System.Net.HttpStatusCode.UnprocessableEntity);
            }

            var user = await _userManager.GetUserAsync(_contextAccessor.HttpContext!.User);

            if (user is null)
            {
                return await Response.FailureAsync("Unauthorized.", System.Net.HttpStatusCode.Unauthorized);
            }

            var head = await _unitOfWork.Heads.GetByIdAsync(user.Id);

            if (head is null)
            {
                return await Response.FailureAsync("Unauthorized.", System.Net.HttpStatusCode.Unauthorized);
            }

            if(await _unitOfWork.Repository<Sheet>().Entities
                .AnyAsync(x => (x.Name == command.Name || x.SheetCodefroceId == command.SheetCodefroceId)&&x.CampId==head.CampId))
            {
                return await Response.FailureAsync("sheet already exist.");
            }

            var sheet = command.Adapt<Sheet>();
            sheet.CampId = head.CampId;

            sheet.SheetOrder = await _unitOfWork.Repository<Sheet>().Entities.Where(x => x.CampId == head.CampId).CountAsync(cancellationToken) + 1;

            await _unitOfWork.Repository<Sheet>().AddAsync(sheet);
            await _unitOfWork.SaveAsync();

            return await Response.SuccessAsync(sheet.Id, "Sheet added.");
        }
    }
}
