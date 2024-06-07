using ISc.Application.Interfaces.Repos;
using ISc.Domain.Models;
using ISc.Shared;
using Mapster;
using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ISc.Application.Features.Leader.Request.Queries.DisplayOnSystemFilter
{
    public record GetRegisterationOnSystemFilterQuery : IRequest<Response>
    {
        public int CampId { get; set; }

        public GetRegisterationOnSystemFilterQuery(int campId)
        {
            CampId = campId;
        }
    }

    internal class DisplayOnSystemFilterQueryHandler : IRequestHandler<GetRegisterationOnSystemFilterQuery, Response>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DisplayOnSystemFilterQueryHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Response> Handle(GetRegisterationOnSystemFilterQuery query, CancellationToken cancellationToken)
        {
            var camp = await _unitOfWork.Repository<Camp>().GetByIdAsync(query.CampId);

            if (camp == null)
            {
                return await Response.FailureAsync("Camp not found.", System.Net.HttpStatusCode.NotFound);
            }

            var archive = await _unitOfWork.Repository<TraineeArchive>().Entities
                                .Where(x => x.CampName.Trim().ToLower() == camp.Name.Trim().ToLower())
                                .ToListAsync(cancellationToken);

            var campRequests = await _unitOfWork.Repository<NewRegisteration>().Entities
                            .Where(x => x.CampId == query.CampId)
                            .Where(x => !archive.Any(r => (r.FirstName + r.MiddleName + r.LastName).Trim().ToLower() == (x.FirstName + x.MiddleName + x.LastName).Trim().ToLower()))
                            .Where(x => !archive.Any(r => r.NationalId == x.NationalId))
                            .Where(x => !archive.Any(r => r.CodeForceHandle == x.CodeForceHandle))
                            .Where(x => !archive.Any(r => r.PhoneNumber == x.PhoneNumber))
                            .Where(x => !archive.Any(r => r.Email == x.Email))
                            .ProjectToType<GetRegisterationOnSystemFilterQueryDto>(_mapper.Config)
                            .ToListAsync(cancellationToken);

            return await Response.SuccessAsync(campRequests);
        }
    }
}
