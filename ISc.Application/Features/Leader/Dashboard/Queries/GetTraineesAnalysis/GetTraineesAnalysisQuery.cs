using ISc.Application.Features.Leader.Dashboard.Queries.GetCampsAnalysis;
using ISc.Application.Interfaces.Repos;
using ISc.Domain.Comman.Enums;
using ISc.Domain.Models;
using ISc.Shared;
using Mapster;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ISc.Application.Features.Leader.Dashboard.Queries.GetTraineesAnalysis
{
	public record GetTraineesAnalysisQuery : IRequest<Response>;
	internal class GetTraineesAnalysisQueryHandler : IRequestHandler<GetTraineesAnalysisQuery, Response>
	{
		private readonly IUnitOfWork _unitOfWork;

		public GetTraineesAnalysisQueryHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}
		public async Task<Response> Handle(GetTraineesAnalysisQuery request, CancellationToken cancellationToken)
		{
			GetTraineesAnalysisQueryDto traineesAnalysis = new GetTraineesAnalysisQueryDto();

			traineesAnalysis.NumberOfTrainees = _unitOfWork.Trainees.Entities
				.Select(i => i).Count();

			traineesAnalysis.NumberOfMaleTrainees = _unitOfWork.Trainees.Entities
				.Where(t=>t.Account.Gender==Gender.male).Count();

			traineesAnalysis.NumberOfFemaleTrainees = _unitOfWork.Trainees.Entities
				.Where(t=>t.Account.Gender==Gender.female).Count();

			traineesAnalysis.CollegesAnalisis = _unitOfWork.Trainees.Entities
				.GroupBy(p => p.Account.College)
				.Select(p => new { Name = p.Key, Count = p.Count() }).ProjectToType<CollegeAnalisisDto>().ToList();

			return await Response.SuccessAsync(traineesAnalysis);

		}
	}
}
