using FluentValidation;
using ISc.Application.Interfaces.Repos;
using ISc.Domain.Comman.Enums;
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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace ISc.Application.Features.HeadOfCamps.Sheets.Commands.Update
{
    public record UpdateSheetCommand:IRequest<Response>
    {
        public int id { get; set; }
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

    internal class UpdateSheetCommandHandler : IRequestHandler<UpdateSheetCommand, Response>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly UserManager<Account> _userManager;
        private readonly IValidator<UpdateSheetCommand> _validator;
        private readonly IMapper _mapper;
        public UpdateSheetCommandHandler(
            IUnitOfWork unitOfWork,
            IHttpContextAccessor contextAccessor,
            UserManager<Account> userManager,
            IValidator<UpdateSheetCommand> validator,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _contextAccessor = contextAccessor;
            _userManager = userManager;
            _validator = validator;
            _mapper = mapper;
        }

        public async Task<Response> Handle(UpdateSheetCommand command, CancellationToken cancellationToken)
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

            var sheet = await _unitOfWork.Repository<Sheet>().GetByIdAsync(command.id);

            if(sheet is null)
            {
                return await Response.FailureAsync("Sheet not found.", System.Net.HttpStatusCode.NotFound);
            }

            if(sheet.CampId != head.CampId)
            {
                return await Response.FailureAsync("Unauthorized.", System.Net.HttpStatusCode.Unauthorized);

            }

            _mapper.Map(command,sheet);

            await _unitOfWork.Repository<Sheet>().UpdateAsync(sheet);
            await _unitOfWork.SaveAsync();

            return await Response.SuccessAsync(sheet.Id, "Sheet updated");
        }
    }
}
