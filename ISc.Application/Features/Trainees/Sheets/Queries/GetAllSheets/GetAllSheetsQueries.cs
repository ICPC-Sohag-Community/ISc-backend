using ISc.Application.Interfaces.Repos;
using ISc.Domain.Comman.Enums;
using ISc.Domain.Models;
using ISc.Domain.Models.IdentityModels;
using ISc.Shared;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Net;

namespace ISc.Application.Features.Trainees.Sheets.Queries.GetAllSheets
{
    public record GetAllSheetsQueries : IRequest<Response>
    {
    }

    internal class GetAllSheetsQueriesHandler : IRequestHandler<GetAllSheetsQueries, Response>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly UserManager<Account> _userManager;

        public GetAllSheetsQueriesHandler(
            IUnitOfWork unitOfWork,
            IHttpContextAccessor contextAccessor,
            UserManager<Account> userManager)
        {
            _unitOfWork = unitOfWork;
            _contextAccessor = contextAccessor;
            _userManager = userManager;
        }

        public async Task<Response> Handle(GetAllSheetsQueries request, CancellationToken cancellationToken)
        {
            var user = await _userManager.GetUserAsync(_contextAccessor.HttpContext!.User);

            if (user is null)
            {
                return await Response.FailureAsync("Unauthorized.", HttpStatusCode.Unauthorized);
            }

            var trainee = await _unitOfWork.Trainees.GetByIdAsync(user.Id);

            if (trainee is null)
            {
                return await Response.FailureAsync("Unauthorized.", HttpStatusCode.Unauthorized);
            }

            var sheets = _unitOfWork.Repository<Sheet>().Entities.Where(x => x.CampId == trainee.CampId).OrderBy(x => x.SheetOrder).ToList();
            var traineeSolve = _unitOfWork.Repository<TraineeAccessSheet>().Entities.Where(x => x.TraineeId == trainee.Id).ToList();

            var traineeSheets = new List<GetAllSheetsQueriesDto>();

            foreach (var sheet in sheets)
            {
                var entity = sheet.Adapt<GetAllSheetsQueriesDto>();
                entity.Date = sheet.Status == SheetStatus.InComing ? sheet.StartDate : sheet.EndDate;
                entity.ProblemSolved = traineeSolve.Count(x => x.SheetId == sheet.Id);
                entity.SolvedPrecent = entity.ProblemSolved / entity.ProblemCount * 100;

                traineeSheets.Add(entity);
            }

            return await Response.SuccessAsync(traineeSheets);
        }
    }
}
