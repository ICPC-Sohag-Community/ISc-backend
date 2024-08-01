using ISc.Application.Extension;
using ISc.Application.Interfaces.Repos;
using ISc.Domain.Comman.Enums;
using ISc.Domain.Models;
using ISc.Shared;
using Mapster;
using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace ISc.Application.Features.Leader.Request.Queries.DisplayAll
{
    public record GetAllRegisterationQuery : PaginatedRequest, IRequest<PaginatedRespnose<GetAllRegisterationQueryDto>>
    {
        public int CampId { get; set; }
        public GetAllRegisterationQueryDtoColumn? SortBy { get; set; }
        public bool ApplySystemFilter { get; set; }
        public List<FilterationModel>? Filters { get; set; }
    }
    internal class GetAllRegisterationQueryHandler : IRequestHandler<GetAllRegisterationQuery, PaginatedRespnose<GetAllRegisterationQueryDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public GetAllRegisterationQueryHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IMediator mediator)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task<PaginatedRespnose<GetAllRegisterationQueryDto>> Handle(GetAllRegisterationQuery query, CancellationToken cancellationToken)
        {
            var camp = await _unitOfWork.Repository<Camp>().GetByIdAsync(query.CampId);

            if (camp == null)
            {
                return await PaginatedRespnose<GetAllRegisterationQueryDto>.FailureAsync("Camp not found.", System.Net.HttpStatusCode.NotFound);
            }

            var entities = _unitOfWork.Repository<NewRegisteration>().Entities
                        .Where(x => x.CampId == query.CampId);

            if (query.SortBy is not null)
            {
                if (query.SortBy == GetAllRegisterationQueryDtoColumn.Gender)
                {
                    entities = entities.OrderBy(x => x.Gender);
                }
                else if (query.SortBy == GetAllRegisterationQueryDtoColumn.Year)
                {
                    entities = entities.OrderBy(x => x.Grade);
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

            if (query.ApplySystemFilter)
            {
                var archive = await _unitOfWork.Repository<TraineeArchive>().Entities
                                .Where(x => x.CampName.Trim().ToLower() == camp.Name.Trim().ToLower())
                                .ToListAsync(cancellationToken);

                 entities = _unitOfWork.Repository<NewRegisteration>().Entities
                                .Where(x => x.CampId == query.CampId)
                                .Where(x => !archive.Any(r => (r.FirstName + r.MiddleName + r.LastName).Trim().ToLower() == (x.FirstName + x.MiddleName + x.LastName).Trim().ToLower()))
                                .Where(x => !archive.Any(r => r.NationalId == x.NationalId))
                                .Where(x => !archive.Any(r => r.CodeForceHandle == x.CodeForceHandle))
                                .Where(x => !archive.Any(r => r.PhoneNumber == x.PhoneNumber))
                                .Where(x => !archive.Any(r => r.Email == x.Email));
            }

            if (!query.Filters.IsNullOrEmpty())
            {
                var customerFilters = await _mediator.Send(new GetOnCustomerFilterQuery()
                {
                    Filters=query.Filters!,
                    Registeration=entities
                },cancellationToken);

                var registerations = customerFilters.Skip((query.PageNumber - 1) * query.PageSize).Take(query.PageSize).ToList();

                return PaginatedRespnose<GetAllRegisterationQueryDto>.Create(registerations, customerFilters.Count, query.PageNumber, query.PageSize);
            }

            return await entities.ProjectToType<GetAllRegisterationQueryDto>(_mapper.Config)
                                 .ToPaginatedListAsync(query.PageNumber,query.PageSize,cancellationToken);
        }
    }
}