using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISc.Application.Features.Leader.Camps.Queries.GetCampEditById;
using ISc.Application.Interfaces.Repos;
using ISc.Domain.Models;
using ISc.Shared;
using Mapster;
using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ISc.Application.Features.Leader.Request.Queries.DisplayAll
{
    public record GetAllRegisterationQuery : IRequest<Response>
    {
        public int CampId { get; set; }

        public GetAllRegisterationQuery(int campId)
        {
            CampId = campId;
        }
    }

    internal class DisplayAllRegisterationQueryHandler : IRequestHandler<GetAllRegisterationQuery, Response>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DisplayAllRegisterationQueryHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Response> Handle(GetAllRegisterationQuery query, CancellationToken cancellationToken)
        {
            var entities = await _unitOfWork.Repository<NewRegisteration>().Entities
                        .Where(x => x.CampId == query.CampId)
                        .ProjectToType<GetAllRegisterationQueryDto>(_mapper.Config)
                        .ToListAsync(cancellationToken);

            return await Response.SuccessAsync(entities);
        }
    }
}
