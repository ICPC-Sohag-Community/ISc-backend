using ISc.Application.Interfaces.Repos;
using ISc.Shared;
using Mapster;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISc.Application.Features.Leader.Standing.Queries.GetStandingByCampId
{
    public record GetStandingByCampIdQuery : IRequest<Response>
    {
        public int CampId { get; set; }

        public GetStandingByCampIdQuery(int campId)
        {
            CampId = campId;
        }
    }

    internal class GetStandingByCampIdQueryHandler : IRequestHandler<GetStandingByCampIdQuery, Response>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetStandingByCampIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Response> Handle(GetStandingByCampIdQuery query, CancellationToken cancellationToken)
        {
            var entities = await _unitOfWork.GetStandingAsync(query.CampId);

            var standing = entities.Adapt<List<GetStandingByCampIdQueryDto>>();

            return await Response.SuccessAsync(standing);
        }
    }
}
