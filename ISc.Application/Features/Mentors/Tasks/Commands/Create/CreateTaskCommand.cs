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
using System.Net;

namespace ISc.Application.Features.Mentors.Tasks.Commands.Create
{
    public record CreateTaskCommand : IRequest<Response>
    {
        public string Title { get; set; }
        public DateTime DeadLine { get; set; }
        public string? TraineeId { get; set; }
        public int CampId { get; set; }
        public List<string> TaskMissions { get; set; }
    }
    internal class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, Response>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<CreateTaskCommand> _validator;
        private readonly UserManager<Account> _userManager;
        private readonly IHttpContextAccessor _contextAccessor;

        public CreateTaskCommandHandler(
            IUnitOfWork unitOfWork,
            IValidator<CreateTaskCommand> validator,
            UserManager<Account> userManager,
            IHttpContextAccessor contextAccessor)
        {
            _unitOfWork = unitOfWork;
            _validator = validator;
            _userManager = userManager;
            _contextAccessor = contextAccessor;
        }

        public async Task<Response> Handle(CreateTaskCommand command, CancellationToken cancellationToken)
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

            var task = command.Adapt<TraineeTask>();
            task.Status = TasksStatus.ToDo;
            task.Missions = command.TaskMissions.Select(x => new TaskMission()
            {
                Task = x,
                TraineeTaskId = task.Id
            }).ToList();

            if (command.TraineeId is null)
            {
                var traineesIds = await _unitOfWork.Trainees.Entities
                            .Where(x => x.CampId == command.CampId && x.MentorId == user!.Id)
                            .Select(x => x.Id)
                            .ToListAsync();

                foreach (var traineeId in traineesIds)
                {
                    var traineeTask = task;
                    traineeTask!.TraineeId = traineeId;

                    await _unitOfWork.Repository<TraineeTask>().AddAsync(traineeTask);
                }
            }
            else
            {
                task.TraineeId = command.TraineeId;
                await _unitOfWork.Repository<TraineeTask>().AddAsync(task);
            }

            await _unitOfWork.SaveAsync();

            return await Response.SuccessAsync("Task Posted");
        }
    }
}
