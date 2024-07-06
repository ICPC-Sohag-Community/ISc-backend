using ISc.Application.Interfaces.Repos;
using ISc.Domain.Models;
using ISc.Shared;
using MediatR;

namespace ISc.Application.Features.Mobile.Queries.GetCamps
{
    public class GetCampsQuery : IRequest<Response>;

    public class GetCampsQueryHandler : IRequestHandler<GetCampsQuery, Response>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetCampsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Response> Handle(GetCampsQuery query, CancellationToken cancellationToken)
        {
            var camps = _unitOfWork.Repository<Camp>().Entities.Select(i => new GetCampsQueryDto { Id = i.Id, Name = i.Name });

            return await Response.SuccessAsync(camps, "Success");
        }
    }
}
