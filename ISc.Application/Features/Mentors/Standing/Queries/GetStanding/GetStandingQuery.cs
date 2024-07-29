using ISc.Application.Interfaces.Repos;
using ISc.Shared;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISc.Application.Features.Mentors.Standing.Queries.GetStanding
{
    public class GetStandingQuery:IRequest<Response>
    {
        public int CampId { get; set; }

        public GetStandingQuery(int campId)
        {
            CampId = campId;
        }
    }

    internal class GetStandingQueryHandler : IRequestHandler<GetStandingQuery, Response>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetStandingQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Response> Handle(GetStandingQuery query, CancellationToken cancellationToken)
        {
            var entity = await _unitOfWork.GetStandingAsync(query.CampId);

            var standing = entity.Adapt<GetStandingQueryDto>();

            return await Response.SuccessAsync(standing);
        }
    }
}
