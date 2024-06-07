using ISc.Application.Interfaces;
using ISc.Application.Interfaces.Repos;
using ISc.Domain.Models;
using ISc.Shared;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ISc.Application.Features.Leader.Request.Queries.DisplayById
{
    public record GetRegisterationByIdQuery : IRequest<Response>
    {
        public string NationalId { get; set; }
        public int CampId { get; set; }
    }

    internal class DisplayAllRegisterationQueryHandler : IRequestHandler<GetRegisterationByIdQuery, Response>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediaServices _mediaServices;

        public DisplayAllRegisterationQueryHandler(
            IUnitOfWork unitOfWork,
            IMediaServices mediaServices)
        {
            _unitOfWork = unitOfWork;
            _mediaServices = mediaServices;
        }

        public async Task<Response> Handle(GetRegisterationByIdQuery query, CancellationToken cancellationToken)
        {
            var entity = await _unitOfWork.Repository<NewRegisteration>().Entities
                        .SingleOrDefaultAsync(x => x.CampId == query.CampId && x.NationalId == query.NationalId);

            if (entity is null)
            {
                return await Response.FailureAsync("Request not found.", System.Net.HttpStatusCode.NotFound);
            }

            entity.ImageUrl = _mediaServices.GetUrl(entity.ImageUrl);

            var request = entity.Adapt<GetRegisterationByIdQueryDto>();

            return await Response.SuccessAsync(request);
        }
    }
}
