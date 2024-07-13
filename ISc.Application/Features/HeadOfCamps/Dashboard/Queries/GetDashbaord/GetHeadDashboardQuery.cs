using ISc.Application.Interfaces.Repos;
using ISc.Domain.Models;
using ISc.Domain.Models.IdentityModels;
using ISc.Shared;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ISc.Application.Features.HeadOfCamps.Dashboard.Queries.GetDashbaord
{
    public record GetHeadDashboardQuery : IRequest<Response>;

    internal class GetDashboardQueryHandler : IRequestHandler<GetHeadDashboardQuery, Response>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly UserManager<Account> _userManager;

        public GetDashboardQueryHandler(
            IUnitOfWork unitOfWork,
            IHttpContextAccessor contextAccessor,
            UserManager<Account> userManager)
        {
            _unitOfWork = unitOfWork;
            _contextAccessor = contextAccessor;
            _userManager = userManager;
        }

        public async Task<Response> Handle(GetHeadDashboardQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.GetUserAsync(_contextAccessor.HttpContext!.User);

            if (user is null)
            {
                return await Response.FailureAsync("Unauthorized.", System.Net.HttpStatusCode.Unauthorized);
            }

            var head = await _unitOfWork.Heads.GetByIdAsync(user.Id);

            if (head is null)
            {
                return await Response.FailureAsync("Unauthorized.", System.Net.HttpStatusCode.Unauthorized);
            }

            var camp = head.Camp;
            var trainees = _unitOfWork.Trainees.Entities
                                    .Where(x => x.CampId == head.CampId)
                                    .GroupBy(x => x.Account.Gender)
                                    .Select(x => new
                                    {
                                        x.Key,
                                        Count = x.Count()
                                    }).ToDictionary(x => x.Key, x => x.Count);

            var mentors = _unitOfWork.Mentors.Entities
                                    .Where(x => x.Camps.Select(c => c.CampId).Contains(head.CampId))
                                    .GroupBy(x => x.Account.Gender)
                                    .Select(x => new
                                    {
                                        x.Key,
                                        Count = x.Count()
                                    }).ToDictionary(x => x.Key, x => x.Count);

            var analysis = new GetHeadDashboardQueryDto()
            {
                TrineesMaleCount = trainees.TryGetValue(Domain.Comman.Enums.Gender.Male, out int maleTrainee) ? maleTrainee : 0,
                TraineesFemaleCount = trainees.TryGetValue(Domain.Comman.Enums.Gender.Female, out int femaleTrainees) ? femaleTrainees : 0,
                MentorsMaleCount = mentors.TryGetValue(Domain.Comman.Enums.Gender.Male, out int maleMentors) ? maleMentors : 0,
                MentorsFemaleCount = mentors.TryGetValue(Domain.Comman.Enums.Gender.Female, out int femaleMentors) ? femaleMentors : 0,
                RemainDays = (DateTime.Parse(camp.EndDate.ToString()) - DateTime.Parse(camp.startDate.ToString())).Days,
                SheetsAnalysis = await SheetsAverage(camp),
                ContestsAnalysis = await ContestAverage(camp)
            };

            return await Response.SuccessAsync(analysis);

        }

        private async Task<List<SheetAverageSolvingDto>> SheetsAverage(Camp camp)
        {
            var sheets = camp.Sheets.OrderBy(x => x.SheetOrder);

            var averages = new List<SheetAverageSolvingDto>();

            foreach (var sheet in sheets)
            {
                averages.Add(new()
                {
                    Name = sheet.Name,
                    Precent = (await _unitOfWork.Repository<TraineeAccessSheet>().Entities.Where(x => x.SheetId == sheet.Id).CountAsync() / sheet.ProblemCount) * 100,
                });
            }

            return averages;
        }

        private async Task<List<ContestAverageSolvingDto>> ContestAverage(Camp camp)
        {
            var sheets = camp.Sheets.OrderBy(x => x.SheetOrder);

            var averages = new List<ContestAverageSolvingDto>();

            foreach (var contest in sheets)
            {
                averages.Add(new()
                {
                    Name = contest.Name,
                    Precent = (await _unitOfWork.Repository<TraineeAccessContest>().Entities.Where(x => x.ContestId == contest.Id).CountAsync() / contest.ProblemCount) * 100,
                });
            }

            return averages;
        }
    }
}
