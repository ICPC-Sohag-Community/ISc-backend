using ISc.Application.Features.Leader.Request.Queries.DisplayAll;
using ISc.Application.Interfaces;
using ISc.Application.Interfaces.Repos;
using ISc.Domain.Models;
using ISc.Shared;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

namespace ISc.Application.Features.Leader.Request.Queries.DisplayById
{
    public record DisplayRegisterationByIdQuery:IRequest<Response>
    {
        public string NationalId { get; set; }
        public int CampId { get; set; }
    }

    internal class DisplayAllRegisterationQueryHandler : IRequestHandler<DisplayRegisterationByIdQuery, Response>
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

        public async Task<Response> Handle(DisplayRegisterationByIdQuery query, CancellationToken cancellationToken)
        {
            var entity = await _unitOfWork.Repository<NewRegisteration>().Entities
                        .SingleOrDefaultAsync(x => x.CampId == query.CampId && x.NationalId == query.NationalId);

            if(entity is null)
            {
                return await Response.FailureAsync("Request not found.", System.Net.HttpStatusCode.NotFound);
            }

            entity.ImageUrl = _mediaServices.GetUrl(entity.ImageUrl);

            var request = entity.Adapt<DisplayRegisterationByIdQueryDto>();

            return await Response.SuccessAsync(request);
        }
    }
}
