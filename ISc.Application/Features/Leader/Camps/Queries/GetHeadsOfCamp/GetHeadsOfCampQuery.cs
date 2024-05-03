using ISc.Application.Features.Leader.Camps.Queries.GetAllMentor;
using ISc.Application.Interfaces.Repos;
using ISc.Shared;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISc.Application.Features.Leader.Camps.Queries.GetHeadsOfCamp
{
	public record GetHeadsOfCampQuery : IRequest<Response>;
	internal class GetHeadsOfCampQueryHandler : IRequestHandler<GetHeadsOfCampQuery, Response>
	{
		private readonly IUnitOfWork _unitOfWork;

		public GetHeadsOfCampQueryHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<Response> Handle(GetHeadsOfCampQuery query, CancellationToken cancellationToken)
		{
			var Heads = await _unitOfWork.Mentors.Entities
							 .Select(m => new
							 {
								 m.Id,
								 FullName = $"{m.Account.FirstName} {m.Account.MiddleName} {m.Account.LastName}"
							 })
							 .ProjectToType<GetHeadsOfCampQueryDto>()
							 .ToListAsync();

			return await Response.SuccessAsync(Heads);
		}
	}
}
