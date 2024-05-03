using ISc.Application.Interfaces.Repos;
using ISc.Shared;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ISc.Application.Features.Leader.Camps.Queries.GetAllMentor
{
    public record GetAllMentorQuery : IRequest<Response>;
    internal class GetAllMentorQueryHandler : IRequestHandler<GetAllMentorQuery, Response>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllMentorQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Response> Handle(GetAllMentorQuery query, CancellationToken cancellationToken)
        {
            var mentors = await _unitOfWork.Mentors.Entities
                                .Select(m => new
                                {
                                    m.Id,
                                    FullName = $"{m.Account.FirstName} {m.Account.MiddleName} {m.Account.LastName}"
                                })
                                .ProjectToType<GetAllMentorQueryDto>()
                                .ToListAsync();
                        
            return await Response.SuccessAsync(mentors);
        }
    }
}
