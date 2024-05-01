using ISc.Application.Interfaces.Repos;
using ISc.Domain.Comman.Enums;
using ISc.Domain.Models;
using ISc.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ISc.Application.Features.Leader.Dashboard.Queries.GetCampsAnalysis
{
    public record GetCampsAnalysisQuery : IRequest<Response>;

    internal class GetCampsAnalysisQueryHandler : IRequestHandler<GetCampsAnalysisQuery, Response>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetCampsAnalysisQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Response> Handle(GetCampsAnalysisQuery request, CancellationToken cancellationToken)
        {
            var camps = await _unitOfWork.Repository<Camp>().GetAllAsync();

            var campsAnalysis = new List<GetCampsAnalysisQueryDto>();

            foreach (var camp in camps)
            {
                var completedSheets = camp.Sheets
                    .Where(x => x.Status == SheetStatus.Completed)
                    .ToList();

                var trainees = camp.Trainees.Count;

                var solvedProblems = await _unitOfWork.Repository<TraineeAccessSheet>().Entities
                                    .Where(x => camp.Sheets.Select(s => s.Id).Contains(x.SheetId))
                                    .CountAsync();

                campsAnalysis.Add(new()
                {
                    Id = camp.Id,
                    Name = camp.Name,
                    DueDate = camp.EndDate,
                    Progress = (solvedProblems / (completedSheets.Count * trainees)) * 100
                });
            }

            return await Response.SuccessAsync(campsAnalysis);
        }
    }
}
