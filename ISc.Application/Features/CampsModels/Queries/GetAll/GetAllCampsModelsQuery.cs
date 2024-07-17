using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISc.Application.Interfaces.Repos;
using ISc.Domain.Models;
using ISc.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace ISc.Application.Features.CampsModels.Queries.GetAll
{
    public record GetAllCampsModelsQuery:IRequest<Response>
    {
        public string? KeyWord { get;set; }
    }

    internal class GetAllCampsModelsQueryHandler : IRequestHandler<GetAllCampsModelsQuery, Response>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllCampsModelsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Response> Handle(GetAllCampsModelsQuery query, CancellationToken cancellationToken)
        {
            var entities = _unitOfWork.Repository<CampModel>();

            if (query.KeyWord.IsNullOrEmpty())
            {
                return await Response.SuccessAsync(await entities.GetAllAsync());
            }

            var modles = await entities.Entities
                             .Where(x => x.Name.ToLower().Trim().Replace(" ", "").StartsWith(query.KeyWord.Trim().Replace(" ", "").ToLower()))
                             .ToListAsync(cancellationToken);

            return await Response.SuccessAsync(modles!);
        }
    }
}
