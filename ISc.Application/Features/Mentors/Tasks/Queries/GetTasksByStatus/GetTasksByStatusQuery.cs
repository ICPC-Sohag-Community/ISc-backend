using ISc.Application.Interfaces.Repos;
using ISc.Domain.Models;
using ISc.Domain.Models.IdentityModels;
using ISc.Shared;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace ISc.Application.Features.Mentors.Tasks.Queries.GetTasksByStatus
{
    public record GetTasksByStatusQuery : IRequest<Response>
    {
        public int CampId { get; set; }

        public GetTasksByStatusQuery(int campId)
        {
            CampId = campId;
        }
    }

    internal class GetQueriesByStatusQueryHandler : IRequestHandler<GetTasksByStatusQuery, Response>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly UserManager<Account> _userManager;

        public GetQueriesByStatusQueryHandler(
            IUnitOfWork unitOfWork,
            IHttpContextAccessor contextAccessor,
            UserManager<Account> userManager)
        {
            _unitOfWork = unitOfWork;
            _contextAccessor = contextAccessor;
            _userManager = userManager;
        }

        public async Task<Response> Handle(GetTasksByStatusQuery query, CancellationToken cancellationToken)
        {
            var mentor = await _userManager.GetUserAsync(_contextAccessor.HttpContext!.User);

            if (mentor is null)
            {
                return await Response.FailureAsync("Unauthorized.", HttpStatusCode.Unauthorized);
            }

            var entities = await _unitOfWork.Repository<TraineeTask>().Entities
                         .Where(x => x.Trainee.MentorId == mentor.Id && x.Trainee.CampId == query.CampId)
                         .GroupBy(x => x.Status)
                         .ToListAsync();

            var tasks = new List<GetTasksByStatusQueryDto>();

            foreach (var entity in entities)
            {
                tasks.Add(new()
                {
                    Status = entity.Key,
                    Trainees = entity.Select(x => new GetTasksByStatusTraineeInfoDto()
                    {
                        DeadLine = x.DeadLine,
                        FirstName = x.Trainee.Account.FirstName,
                        MiddleName = x.Trainee.Account.MiddleName,
                        LastName = x.Trainee.Account.LastName,
                        PhotoUrl = x.Trainee.Account.PhotoUrl,
                        Task = x.Task
                    }).ToList()
                });
            }

            return await Response.SuccessAsync(tasks);
        }
    }
}
