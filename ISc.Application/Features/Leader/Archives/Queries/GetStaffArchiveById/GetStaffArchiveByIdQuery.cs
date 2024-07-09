using ISc.Application.Interfaces.Repos;
using ISc.Domain.Models;
using ISc.Shared;
using Mapster;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ISc.Application.Features.Leader.Archives.Queries.GetStaffArchiveById
{
	public record GetStaffArchiveByIdQuery:IRequest<Response>
	{
        public int Id { get; set; }

        public GetStaffArchiveByIdQuery(int id)
        {
            Id = id;
        }
    }
	internal class GetStaffArchiveByIdQueryHandler : IRequestHandler<GetStaffArchiveByIdQuery, Response>
	{
		private readonly IUnitOfWork _unitOfWork;

		public GetStaffArchiveByIdQueryHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<Response> Handle(GetStaffArchiveByIdQuery query, CancellationToken cancellationToken)
		{
			var entity = await _unitOfWork.Repository<StuffArchive>().GetByIdAsync(query.Id);
			
			if(entity==null)
			{
				return await Response.FailureAsync("Archive not found.", HttpStatusCode.NotFound);

			}
			var archive = entity.Adapt<GetStaffArchiveByIdQueryDto>();

			return await Response.SuccessAsync(archive);

		}
	}
}
