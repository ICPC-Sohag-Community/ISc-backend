using FluentValidation;
using ISc.Application.Interfaces;
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
using System.Net;

namespace ISc.Application.Features.HeadOfCamps.Contests.Commands.Create
{
    public class CreatecontestCommand : IRequest<Response>
    {
        public string Name { get; set; }
        public string Link { get; set; }
        public Community Community { get; set; }
        public int ProblemCount { get; set; }
        public string CodeForceId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }

    internal class CreatecontestCommandHandler : IRequestHandler<CreatecontestCommand, Response>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJobServices _jobServices;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly UserManager<Account> _userManager;
        private readonly IValidator<CreatecontestCommand> _validator;

        public CreatecontestCommandHandler(
            IUnitOfWork unitOfWork,
            IValidator<CreatecontestCommand> validator,
            IHttpContextAccessor contextAccessor,
            UserManager<Account> userManager,
            IJobServices jobServices)
        {
            _unitOfWork = unitOfWork;
            _validator = validator;
            _contextAccessor = contextAccessor;
            _userManager = userManager;
            _jobServices = jobServices;
        }

        public async Task<Response> Handle(CreatecontestCommand command, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(command, cancellationToken);

            if (!validationResult.IsValid)
            {
                return await Response.ValidationFailureAsync(validationResult.Errors.ToList(), HttpStatusCode.UnprocessableEntity);
            }

            var user = await _userManager.GetUserAsync(_contextAccessor.HttpContext!.User);

            if (user is null)
            {
                return await Response.FailureAsync("Unauthorized.", HttpStatusCode.Unauthorized);
            }

            var head = await _unitOfWork.Heads.GetByIdAsync(user.Id);

            if (head is null)
            {
                return await Response.FailureAsync("Unauthorized.", HttpStatusCode.Unauthorized);
            }

            if (await _unitOfWork.Repository<Contest>().Entities
                .AnyAsync(x => (x.Name == command.Name || x.Link == command.Link || x.CodeForceId == command.CodeForceId) && x.CampId == head.CampId))
            {
                return await Response.FailureAsync("Camp already exist.", HttpStatusCode.BadRequest);
            }

            var contest = command.Adapt<Contest>();
            contest.CampId = head.CampId;

            await _unitOfWork.Repository<Contest>().AddAsync(contest);
            await _unitOfWork.SaveAsync();

            _jobServices.TrackingContest(contest);

            return await Response.SuccessAsync(contest.Id,"Contest added.");
        }
    }
}