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

namespace ISc.Application.Features.Mentors.Practices.Commands.Create
{
    public class CreatePracticeCommand : IRequest<Response>
    {
        public string Title { get; set; }
        public string MeetingLink { get; set; }
        public string Note { get; set; }
        public DateTime Time { get; set; }
        public int CampId { get; set; }
    }

    internal class CreatePracitceCommandHandler : IRequestHandler<CreatePracticeCommand, Response>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<Account> _userManager;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IValidator<CreatePracticeCommand> _validator;
        public CreatePracitceCommandHandler(
            IUnitOfWork unitOfWork,
            UserManager<Account> userManager,
            IHttpContextAccessor contextAccessor,
            IValidator<CreatePracticeCommand> validator)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _contextAccessor = contextAccessor;
            _validator = validator;
        }

        public async Task<Response> Handle(CreatePracticeCommand command, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(command, cancellationToken);

            if (!validationResult.IsValid)
            {
                return await Response.ValidationFailureAsync(validationResult.Errors.ToList(), System.Net.HttpStatusCode.UnprocessableEntity);
            }

            var mentor = await _userManager.GetUserAsync(_contextAccessor.HttpContext!.User);

            if (mentor is null)
            {
                return await Response.FailureAsync("Unauthorized.", System.Net.HttpStatusCode.Unauthorized);
            }

            var practice = command.Adapt<Practice>();
            practice.State = PracticeStatus.ToDo;

            await _unitOfWork.Repository<Practice>().AddAsync(practice);
            await _unitOfWork.SaveAsync();

            return await Response.SuccessAsync(practice.Id);
        }
    }
}
