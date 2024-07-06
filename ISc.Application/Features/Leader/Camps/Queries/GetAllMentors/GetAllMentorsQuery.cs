using ISc.Application.Interfaces.Repos;
using ISc.Shared;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ISc.Application.Features.Leader.Camps.Queries.GetAllMentor
{
    public record GetAllMentorsQuery : IRequest<Response>;
    internal class GetAllMentorQueryHandler : IRequestHandler<GetAllMentorsQuery, Response>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllMentorQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Response> Handle(GetAllMentorsQuery query, CancellationToken cancellationToken)
        {
            var mentors = await _unitOfWork.Mentors.Entities
                                .Select(m => new
                                {
                                    m.Id,
                                    FullName = $"{m.Account.FirstName} {m.Account.MiddleName} {m.Account.LastName}"
                                })
                                .ProjectToType<GetAllMentorsQueryDto>()
                                .ToListAsync();
                        
            return await Response.SuccessAsync(mentors);
        }
    }
}
