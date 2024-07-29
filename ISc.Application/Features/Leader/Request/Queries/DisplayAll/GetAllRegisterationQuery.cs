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
        public GetAllRegisterationQueryDtoColumn? SortBy { get; set; }

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
            var entities = _unitOfWork.Repository<NewRegisteration>().Entities
                        .Where(x => x.CampId == query.CampId);

            if (query.SortBy is not null)
            {
                if (query.SortBy == GetAllRegisterationQueryDtoColumn.Gender)
                {
                    entities=entities.OrderBy(x=>x.Gender);
                }
                else if (query.SortBy == GetAllRegisterationQueryDtoColumn.Year)
                {
                    entities=entities.OrderBy(x=>x.Grade);
                }
                else if (query.SortBy == GetAllRegisterationQueryDtoColumn.College)
                {
                    entities = entities.OrderBy(x => x.College);
                }
                else if (query.SortBy == GetAllRegisterationQueryDtoColumn.HasLaptop)
                {
                    entities = entities.OrderByDescending(x => x.HasLaptop);
                }
            }

            var requests = await entities.ProjectToType<GetAllRegisterationQueryDto>(_mapper.Config)
                                        .ToListAsync(cancellationToken);

            return await Response.SuccessAsync(requests);
        }
    }
}
