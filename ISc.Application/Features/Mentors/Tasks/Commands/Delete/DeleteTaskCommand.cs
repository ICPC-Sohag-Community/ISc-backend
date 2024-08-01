using ISc.Application.Interfaces.Repos;
using ISc.Domain.Models;
using ISc.Domain.Models.IdentityModels;
using ISc.Shared;
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

namespace ISc.Application.Features.Mentors.Tasks.Commands.Delete
{
    public record DeleteTaskCommand:IRequest<Response>
    {
        public int TaskId { get; set; }
    }

    internal class DeleteTaskCommandHandler : IRequestHandler<DeleteTaskCommand, Response>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly UserManager<Account> _userManager;

        public DeleteTaskCommandHandler(
            IUnitOfWork unitOfWork,
            IHttpContextAccessor contextAccessor,
            UserManager<Account> userManager)
        {
            _unitOfWork = unitOfWork;
            _contextAccessor = contextAccessor;
            _userManager = userManager;
        }

        public async Task<Response> Handle(DeleteTaskCommand command, CancellationToken cancellationToken)
        {
            var user = await _userManager.GetUserAsync(_contextAccessor.HttpContext!.User);

            if (user == null)
            {
                return await Response.FailureAsync("Unauthroized.", HttpStatusCode.Unauthorized);
            }

            var task = await _unitOfWork.Repository<TraineeTask>().Entities
                    .SingleOrDefaultAsync(x => x.Id == command.TaskId && x.Trainee.MentorId == user.Id);

            if(task is null)
            {
                return await Response.FailureAsync("Task not found.", HttpStatusCode.NotFound);
            }

            _unitOfWork.Repository<TraineeTask>().Delete(task);
            await _unitOfWork.SaveAsync();

            return await Response.SuccessAsync("Task deleted.");
        }
    }
}
