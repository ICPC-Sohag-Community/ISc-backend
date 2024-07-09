using ISc.Application.Interfaces.Repos;
using ISc.Domain.Models;
using ISc.Shared;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ISc.Application.Features.Leader.Camps.Queries.GetCampEditById
{
    public record GetCampEditByIdQuery : IRequest<Response>
    {
        public int Id { get; set; }

        public GetCampEditByIdQuery(int id)
        {
            Id = id;
        }
    }

    internal class GetCampEditByIdQueryHandler : IRequestHandler<GetCampEditByIdQuery, Response>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetCampEditByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Response> Handle(GetCampEditByIdQuery command, CancellationToken cancellationToken)
        {
            var entity = await _unitOfWork.Repository<Camp>().GetByIdAsync(command.Id);

            if (entity == null)
            {
                return await Response.FailureAsync("Camp not  found.");
            }

            var mentorsOfCamp = entity.Mentors.Select(x => x.Mentor).ToList();
            var headsOfCamp = entity.Heads.ToList();

            var mentors = await _unitOfWork.Mentors.Entities
                             .Where(x => !mentorsOfCamp.Select(x => x.Id).Contains(x.Id))
                             .Select(x => new GetCampMemeberEditByIdQueryDto()
                             {
                                 Id = x.Id,
                                 Name = x.Account.FirstName + ' ' + x.Account.MiddleName + ' ' + x.Account.LastName,
                                 InCamp = false
                             })
                             .ToListAsync(cancellationToken);

            var heads = await _unitOfWork.Heads.Entities
                            .Where(x => !headsOfCamp.Select(x => x.Id).Contains(x.Id))
                            .Select(x => new GetCampMemeberEditByIdQueryDto()
                            {
                                Id = x.Id,
                                Name = x.Account.FirstName + ' ' + x.Account.MiddleName + ' ' + x.Account.LastName,
                                InCamp = false
                            })
                            .ToListAsync(cancellationToken);

            var camp = entity.Adapt<GetCampEditByIdQueryDto>();

            camp.MentorsOfCamp = mentors;
            camp.MentorsOfCamp.AddRange(mentorsOfCamp.Select(x => new GetCampMemeberEditByIdQueryDto()
            {
                Id = x.Id,
                Name = x.Account.FirstName + ' ' + x.Account.MiddleName + ' ' + x.Account.LastName,
                InCamp = true
            }));

            camp.HeadsOfCamp = heads;
            camp.HeadsOfCamp.AddRange(headsOfCamp.Select(x => new GetCampMemeberEditByIdQueryDto()
            {
                Id = x.Id,
                Name = x.Account.FirstName + ' ' + x.Account.MiddleName + ' ' + x.Account.LastName,
                InCamp = true
            }));

            return await Response.SuccessAsync(camp);
        }
    }
}
