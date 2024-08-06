using ISc.Application.Interfaces.Repos;
using ISc.Domain.Models.IdentityModels;
using ISc.Shared;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Net;

namespace ISc.Application.Features.Trainees.Tasks.Queries.GetTaskWithStatus
{
    public record GetTaskWithStatusQuery : IRequest<Response>;

    internal class GetTaskWithStatusQueryHandler : IRequestHandler<GetTaskWithStatusQuery, Response>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly UserManager<Account> _userManager;

        public GetTaskWithStatusQueryHandler(
            IUnitOfWork unitOfWork,
            IHttpContextAccessor contextAccessor,
            UserManager<Account> userManager)
        {
            _unitOfWork = unitOfWork;
            _contextAccessor = contextAccessor;
            _userManager = userManager;
        }

        public async Task<Response> Handle(GetTaskWithStatusQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.GetUserAsync(_contextAccessor.HttpContext!.User);

            if (user is null)
            {
                return await Response.FailureAsync("Unauthorized.", HttpStatusCode.Unauthorized);
            }

            var trainee = await _unitOfWork.Trainees.GetByIdAsync(user.Id);

            if (trainee == null)
            {
                return await Response.FailureAsync("Unauthorized.", HttpStatusCode.Unauthorized);
            }

            var task = trainee.Tasks.GroupBy(x => x.Status)
                .ToList()
                .Select(x => new GetTaskWithStatusQueryDto()
                {
                    Status = x.Key,
                    Task = x.Adapt<List<TaskDetailDto>>()
                });

            return await Response.SuccessAsync(task);
        }
    }
}
