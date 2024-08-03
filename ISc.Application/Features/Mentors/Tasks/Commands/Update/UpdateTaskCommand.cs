using FluentValidation;
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
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ISc.Application.Features.Mentors.Tasks.Commands.Update
{
    public record UpdateTaskCommand:IRequest<Response>
    {
        public int TaskId { get; set; }
        public string Title { get; set; }
        public DateTime DeadLine { get; set; }
        public string TraineeId { get; set; }
        public List<string> TaskMissions { get; set; }
    }

    internal class UpdateTaskCommandHandler : IRequestHandler<UpdateTaskCommand, Response>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly UserManager<Account> _userManager;
        private readonly IValidator<UpdateTaskCommand> _validator;
        private readonly IMapper _mapper;

        public UpdateTaskCommandHandler(
            IUnitOfWork unitOfWork,
            IHttpContextAccessor contextAccessor,
            UserManager<Account> userManager,
            IValidator<UpdateTaskCommand> validator,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _contextAccessor = contextAccessor;
            _userManager = userManager;
            _validator = validator;
            _mapper = mapper;
        }

        public async Task<Response> Handle(UpdateTaskCommand command, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(command, cancellationToken);

            if (!validationResult.IsValid)
            {
                return await Response.ValidationFailureAsync(validationResult.Errors.ToList(), HttpStatusCode.UnprocessableEntity);
            }

            var user = await _userManager.GetUserAsync(_contextAccessor.HttpContext!.User);

            if (user == null)
            {
                return await Response.FailureAsync("Unauthroized.", HttpStatusCode.Unauthorized);
            }

            var task = await _unitOfWork.Repository<TraineeTask>().Entities
                    .SingleOrDefaultAsync(x => x.Id == command.TaskId && x.Trainee.MentorId == user.Id);

            if (task is null)
            {
                return await Response.FailureAsync("Task not found.", HttpStatusCode.NotFound);
            }

            _mapper.Map(command, task);


            _unitOfWork.Repository<TaskMission>().DeleteRange(task.Missions);
            await _unitOfWork.Repository<TaskMission>().AddRangeAsync(command.TaskMissions.Select(x => new TaskMission()
            {
                Task = x,
                TraineeTaskId = command.TaskId,
            }).ToList());

            await _unitOfWork.Repository<TraineeTask>().UpdateAsync(task);
            await _unitOfWork.SaveAsync();

            return await Response.SuccessAsync(task.Id, "Task updated.");
        }
    }
}
