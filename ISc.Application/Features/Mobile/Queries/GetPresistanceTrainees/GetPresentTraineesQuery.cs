using ISc.Application.Interfaces.Repos;
using ISc.Domain.Models;
using ISc.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace ISc.Application.Features.Mobile.Queries.GetPresistanceTrainees
{
    public record GetPresentTraineesQuery : IRequest<Response>
    {
        public int CampId { get; set; }
        public string? keyWord { get; set; }
        public DateTime CurrentDate { get; set; }
    }

    internal class GetPresistanceTraineesQueryHandler : IRequestHandler<GetPresentTraineesQuery, Response>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetPresistanceTraineesQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Response> Handle(GetPresentTraineesQuery query, CancellationToken cancellationToken)
        {
            var camp = await _unitOfWork.Repository<Camp>().GetByIdAsync(query.CampId);

            if (camp == null)
            {
                return await Response.FailureAsync("Camp not found.");
            }

            var session = camp.Sessions.SingleOrDefault(x => x.StartDate.Date == query.CurrentDate.Date);

            if (session == null)
            {
                return await Response.FailureAsync("No current session for now.");
            }

            var entities = _unitOfWork.Repository<TraineeAttendence>().Entities
                            .Where(x => x.SessionId == session.Id);

            if (!query.keyWord.IsNullOrEmpty())
            {
                entities = entities.Where(x => (x.Trainee.Account.FirstName + x.Trainee.Account.MiddleName + x.Trainee.Account.LastName).Trim().ToLower().StartsWith(query.keyWord!.Replace(" ","").ToLower().Trim())
                                   || x.Trainee.Account.CodeForceHandle.StartsWith(query.keyWord!));
            }

            var attendance = await entities.Select(x => new GetPresentTraineesQueryDto()
            {
                Id = x.TraineeId,
                Name = x.Trainee.Account.FirstName + ' ' + x.Trainee.Account.MiddleName + ' ' + x.Trainee.Account.LastName,
                CodeForceHandle = x.Trainee.Account.CodeForceHandle
            })
                           .ToListAsync(cancellationToken);

            return await Response.SuccessAsync(attendance);
        }
    }
}
