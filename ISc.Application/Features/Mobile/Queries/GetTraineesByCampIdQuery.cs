using ISc.Application.Interfaces.Repos;
using ISc.Domain.Models;
using ISc.Shared;
using MediatR;

namespace ISc.Application.Features.Mobile.Query
{
    public class GetTraineesByCampIdQuery:IRequest<Response>
    {
        public int CampId { get; set; }
    }
    public class GetTraineesByCampIdQueryDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    internal class GetTraineesByCampIdQueryHandler : IRequestHandler<GetTraineesByCampIdQuery, Response>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetTraineesByCampIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Response> Handle(GetTraineesByCampIdQuery query, CancellationToken cancellationToken)
        {
            if (!_unitOfWork.Repository<Camp>().Entities.Any(i => i.Id == query.CampId))
                return await Response.FailureAsync("The Required Camp Is Not Exist");

            var trainees =  _unitOfWork.Trainees.Entities.
                Where(i => i.CampId == query.CampId).Select(i => new GetTraineesByCampIdQueryDto { Id=i.Id, Name=i.Account.FirstName+' '+i.Account.LastName});

            return await Response.SuccessAsync(trainees,"Success"); 
        }
    }
}
