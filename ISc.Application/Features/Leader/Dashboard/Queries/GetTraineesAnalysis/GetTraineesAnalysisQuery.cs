using ISc.Application.Interfaces.Repos;
using ISc.Domain.Comman.Enums;
using ISc.Shared;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

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
		public async Task<Response> Handle(GetTraineesAnalysisQuery query, CancellationToken cancellationToken)
		{

			var traineesAnalysis = await _unitOfWork.Trainees.Entities
				.GroupBy(x => x.Account.Gender).Select(x => new
				{
					Gender = x.Key,
					count = x.Count()
				}).ToDictionaryAsync(x => x.Gender, x => x.count);

			traineesAnalysis.TryGetValue(Gender.female, out int femalesCount);
			traineesAnalysis.TryGetValue(Gender.male, out int malesCount);

			var analysis = new GetTraineesAnalysisQueryDto()
			{
				FemalesCount = femalesCount,
				MalesCount = malesCount,
				CollegesAnalisis = await _unitOfWork.Trainees.Entities
										.GroupBy(p => p.Account.College)
										.Select(p => new { Name = p.Key, Count = p.Count() })
										.ProjectToType<CollegeAnalisisDto>()
										.ToListAsync(cancellationToken: cancellationToken)
			};

			return await Response.SuccessAsync(analysis);

		}
	}
}
