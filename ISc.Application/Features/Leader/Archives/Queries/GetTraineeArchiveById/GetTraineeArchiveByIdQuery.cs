using ISc.Application.Interfaces.Repos;
using ISc.Domain.Models;
using ISc.Shared;
using Mapster;
using MediatR;
using System.Net;

namespace ISc.Application.Features.Leader.Archives.Queries.GetTraineeArchiveById
{
    public record GetTraineeArchiveByIdQuery : IRequest<Response>
    {
        public int Id { get; set; }
        public GetTraineeArchiveByIdQuery(int id)
        {
            Id = id;

        }
    }

    internal class GetTraineeArchiveByIdQueryHandler : IRequestHandler<GetTraineeArchiveByIdQuery, Response>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetTraineeArchiveByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Response> Handle(GetTraineeArchiveByIdQuery query, CancellationToken cancellationToken)
        {
            var entity = await _unitOfWork.Repository<TraineeArchive>().GetByIdAsync(query.Id);

            if (entity is null)
            {
                return await Response.FailureAsync("Archive not found.", HttpStatusCode.NotFound);
            }

            var archive = entity.Adapt<GetTraineeArchiveByIdQueryDto>();

            return await Response.SuccessAsync(archive);
        }
    }
}
