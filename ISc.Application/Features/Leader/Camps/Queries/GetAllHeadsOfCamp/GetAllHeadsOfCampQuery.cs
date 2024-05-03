using ISc.Application.Interfaces.Repos;
using ISc.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ISc.Application.Features.Leader.Camps.Queries.GetAllHeadsOfCamp
{
    public record GetAllHeadsOfCampQuery : IRequest<Response>;
    internal class GetHeadsOfCampQueryHandler : IRequestHandler<GetAllHeadsOfCampQuery, Response>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetHeadsOfCampQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Response> Handle(GetAllHeadsOfCampQuery query, CancellationToken cancellationToken)
        {
            var heads = await _unitOfWork.Heads.Entities
                        .Select(m => new GetAllHeadsOfCampQueryDto()
                        {
                            Id = m.Id,
                            FullName = $"{m.Account.FirstName} {m.Account.MiddleName} {m.Account.LastName}",
                            IsInCamp = true
                        }).ToListAsync();

            var mentors = await _unitOfWork.Mentors.Entities
                              .Where(x => !heads.Select(x => x.Id).Contains(x.Id))
                             .Select(m => new GetAllHeadsOfCampQueryDto()
                             {
                                 Id = m.Id,
                                 FullName = $"{m.Account.FirstName} {m.Account.MiddleName} {m.Account.LastName}",
                                 IsInCamp = false
                             })
                             .ToListAsync(cancellationToken);


            return await Response.SuccessAsync(heads.Concat(mentors));
        }
    }
}
