using ISc.Application.Interfaces.Repos;
using ISc.Domain.Models;
using ISc.Shared;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISc.Application.Features.Camps.Queries.GetCampsOpenRegister
{
    public class GetCampsOpenRegisterQuery:IRequest<Response>;

    internal class GetCampsOpenRegisterQueryHandler : IRequestHandler<GetCampsOpenRegisterQuery, Response>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetCampsOpenRegisterQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Response> Handle(GetCampsOpenRegisterQuery request, CancellationToken cancellationToken)
        {
            var entities = await _unitOfWork.Repository<Camp>().Entities
                          .Where(x => x.OpenForRegister == true)
                          .ToListAsync(cancellationToken);

            var camps=entities.Adapt<List<GetCampsOpenRegisterQueryDto>>();

            return await Response.SuccessAsync(camps);
        }
    }
}
