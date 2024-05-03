using ISc.Application.Interfaces.Repos;
using ISc.Domain.Models;
using ISc.Shared;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ISc.Application.Features.Leader.Dashboard.Queries.GetFeedbacks
{
	public record GetFeedbacksQuery : IRequest<Response>;

	internal class GetFeedbacksQueryHandler : IRequestHandler<GetFeedbacksQuery, Response>
	{
		private readonly IUnitOfWork _unitOfWork;

		public GetFeedbacksQueryHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<Response> Handle(GetFeedbacksQuery query, CancellationToken cancellationToken)
		{
			var feedbacks = await _unitOfWork
				.Repository<SessionFeedback>().Entities
				.OrderBy(x => Guid.NewGuid()).Take(10)
				.Select(p => new
				{
					Rate = p.Rate,
					Feedback = p.Rate,
					PhotoUrl = p.Trainee.Account.PhotoUrl,
					FullName = $"{p.Trainee.Account.FirstName} {p.Trainee.Account.MiddleName}"
				})
				.ProjectToType<GetFeedbacksQueryDto>().ToListAsync();

			return await Response.SuccessAsync(feedbacks);
		}
	}
}
