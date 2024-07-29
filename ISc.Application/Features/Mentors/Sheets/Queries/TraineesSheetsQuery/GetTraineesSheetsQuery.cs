using ISc.Application.Interfaces.Repos;
using ISc.Domain.Comman.Enums;
using ISc.Domain.Models;
using ISc.Domain.Models.IdentityModels;
using ISc.Shared;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ISc.Application.Features.Mentors.Sheets.Queries.TraineesSheetsQuery
{
    public record GetTraineesSheetsQuery : IRequest<Response>
    {
        public int CampId { get; set; }

        public GetTraineesSheetsQuery(int campId)
        {
            CampId = campId;
        }
    }

    internal class GetTraineesSheetsQueryHandler : IRequestHandler<GetTraineesSheetsQuery, Response>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly UserManager<Account> _userManager;

        public GetTraineesSheetsQueryHandler(
            IUnitOfWork unitOfWork,
            IHttpContextAccessor contextAccessor,
            UserManager<Account> userManager)
        {
            _unitOfWork = unitOfWork;
            _contextAccessor = contextAccessor;
            _userManager = userManager;
        }

        public async Task<Response> Handle(GetTraineesSheetsQuery query, CancellationToken cancellationToken)
        {
            var mentor = await _userManager.GetUserAsync(_contextAccessor.HttpContext!.User);

            if (mentor == null)
            {
                return await Response.FailureAsync("Unauthorized.", System.Net.HttpStatusCode.Unauthorized);
            }

            var sheets = await _unitOfWork.Repository<Sheet>().Entities
                        .Where(x => x.CampId == query.CampId && (x.Status == SheetStatus.Completed || x.Status == SheetStatus.InProgress))
                        .ProjectToType<SheetDto>()
                        .ToListAsync();

            var trainees = await _unitOfWork.Trainees.Entities
                          .Where(x => x.MentorId == mentor.Id && x.CampId == query.CampId)
                          .Select(x => x.Account)
                          .ProjectToType<TraineeDto>()
                          .ToListAsync();

            var tracking = _unitOfWork.Repository<TraineeAccessSheet>().Entities
                            .Where(x => x.Trainee.MentorId == mentor.Id && x.Trainee.CampId == query.CampId)
                            .GroupBy(x => new { x.TraineeId, x.SheetId })
                            .Select(x => new
                            {
                                x.Key.TraineeId,
                                x.Key.SheetId,
                                Count = x.Count()
                            })
                            .ToHashSet();

            foreach (var trainee in trainees)
            {
                foreach (var sheet in sheets)
                {
                    var traineeTracking = tracking.SingleOrDefault(x => x.TraineeId == trainee.Id && x.SheetId == sheet.Id);

                    if (traineeTracking is not null)
                    {
                        trainee.Tracking.Add(new()
                        {
                            SheetId = sheet.Id,
                            SolvedProblems = traineeTracking.Count
                        });
                    }
                }
            }

            return await Response.SuccessAsync(new GetTraineesSheetsQueryDto()
            {
                Sheets = sheets,
                Trainees = trainees
            });
        }
    }
}
