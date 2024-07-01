﻿using ISc.Application.Interfaces.Repos;
using ISc.Domain.Models;
using ISc.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ISc.Application.Features.Mobile.Queries.GetTraineesByCampId
{
    public class GetTraineesByCampIdQuery : IRequest<Response>
    {
        public int CampId { get; set; }

        public GetTraineesByCampIdQuery(int campId)
        {
            CampId = campId;
        }
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
            {
                return await Response.FailureAsync("Camp not found.");
            }

            var trainees = await _unitOfWork.Trainees.Entities.
                Where(i => i.CampId == query.CampId)
                .Select(i => new GetTraineesByCampIdQueryDto
                {
                    Id = i.Id,
                    Name = i.Account.FirstName + ' ' + i.Account.MiddleName
                })
                .ToListAsync(cancellationToken);

            return await Response.SuccessAsync(trainees, "Success");
        }
    }
}
