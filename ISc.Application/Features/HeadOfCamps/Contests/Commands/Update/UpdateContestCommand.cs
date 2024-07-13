using FluentValidation;
using ISc.Application.Interfaces;
using ISc.Application.Interfaces.Repos;
using ISc.Domain.Comman.Enums;
using ISc.Domain.Models;
using ISc.Domain.Models.IdentityModels;
using ISc.Shared;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace ISc.Application.Features.HeadOfCamps.Contests.Commands.Update
{
    public record UpdateContestCommand : IRequest<Response>
    {
        public int id { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
        public Community Community { get; set; }
        public int ProblemCount { get; set; }
        public string CodeForceId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }

    internal class UpdateContestCommandHandler : IRequestHandler<UpdateContestCommand, Response>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly UserManager<Account> _userManager;
        private readonly IValidator<UpdateContestCommand> _validator;
        private readonly IJobServices _jobServices;
        private readonly IMapper _mapper;

        public UpdateContestCommandHandler(
            IUnitOfWork unitOfWork,
            IHttpContextAccessor contextAccessor,
            UserManager<Account> userManager,
            IValidator<UpdateContestCommand> validator,
            IMapper mapper,
            IJobServices jobServices)
        {
            _unitOfWork = unitOfWork;
            _contextAccessor = contextAccessor;
            _userManager = userManager;
            _validator = validator;
            _mapper = mapper;
            _jobServices = jobServices;
        }

        public async Task<Response> Handle(UpdateContestCommand command, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(command, cancellationToken);

            if (!validationResult.IsValid)
            {
                return await Response.ValidationFailureAsync(validationResult.Errors.ToList(), System.Net.HttpStatusCode.UnprocessableEntity);
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

            var contest = await _unitOfWork.Repository<Contest>().GetByIdAsync(command.id);

            if (contest is null)
            {
                return await Response.FailureAsync("Contest not found.", HttpStatusCode.BadRequest);
            }

            if (await _unitOfWork.Repository<Contest>().Entities
                .AnyAsync(x => (x.Name == command.Name || x.Link == command.Link) && head.CampId == contest.CampId && contest.Id != command.id))
            {
                return await Response.FailureAsync("Conflict with another contest.", HttpStatusCode.BadRequest);
            }

            if (command.EndTime != contest.EndTime)
            {
                contest.EndTime = command.EndTime;
                _jobServices.TrackingContest(contest);
            }

            _mapper.Map(contest, command);

            await _unitOfWork.Repository<Contest>().UpdateAsync(contest);
            await _unitOfWork.SaveAsync();

            return await Response.SuccessAsync(contest.Id, "Contest updated.");
        }
    }
}
