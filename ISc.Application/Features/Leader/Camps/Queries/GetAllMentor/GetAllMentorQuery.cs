using ISc.Application.Interfaces.Repos;
using ISc.Shared;
using Mapster;
using MediatR;

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
            var mentors = _unitOfWork.Mentors.Entities
                         .Select(m => new
                         {
                             m.Id,
                             FullName = $"{m.Account.FirstName} {m.Account.MiddleName} {m.Account.LastName}"
                         })
                         .ProjectToType<GetAllMentorQueryDto>()
                         .ToList();
            return await Response.SuccessAsync(mentors);

        }
    }
}
