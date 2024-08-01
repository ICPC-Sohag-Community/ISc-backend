using ISc.Application.Interfaces.Repos;
using ISc.Domain.Models.IdentityModels;
using ISc.Shared;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ISc.Application.Features.Mentors.Practices.Queries.GetPractice
{
    public record GetPracticeQuery : IRequest<Response>
    {
        public int CampId { get; set; }

        public GetPracticeQuery(int campId)
        {
            CampId = campId;
        }
    }

    internal class GetPracticeQueryHandler : IRequestHandler<GetPracticeQuery, Response>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly UserManager<Account> _userManager;

        public GetPracticeQueryHandler(
            IUnitOfWork unitOfWork,
            IHttpContextAccessor contextAccessor,
            UserManager<Account> userManager)
        {
            _unitOfWork = unitOfWork;
            _contextAccessor = contextAccessor;
            _userManager = userManager;
        }

        public async Task<Response> Handle(GetPracticeQuery query, CancellationToken cancellationToken)
        {
            var mentor = await _userManager.GetUserAsync(_contextAccessor.HttpContext!.User);

            if (mentor is null)
            {
                return await Response.FailureAsync("Unauthorized.", System.Net.HttpStatusCode.Unauthorized);
            }

            var entities = await _unitOfWork.Repository<Domain.Models.Practice>().Entities
                            .Where(x => x.CampId == query.CampId && x.MentorId == mentor.Id)
                            .OrderByDescending(x=>x.CreationDate)
                            .ToListAsync(cancellationToken);

            var practices = entities.Adapt<List<GetPracticeQueryDto>>();

            return await Response.SuccessAsync(practices);
        }
    }
}