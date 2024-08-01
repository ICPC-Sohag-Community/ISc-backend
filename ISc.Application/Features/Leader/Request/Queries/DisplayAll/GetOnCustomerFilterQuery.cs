using ISc.Application.Dtos.CodeForce;
using ISc.Application.Interfaces;
using ISc.Application.Interfaces.Repos;
using ISc.Domain.Comman.Enums;
using ISc.Domain.Models;
using ISc.Shared;
using Mapster;
using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ISc.Application.Features.Leader.Request.Queries.DisplayAll
{
    public record GetOnCustomerFilterQuery : IRequest<List<GetAllRegisterationQueryDto>>
    {
        public List<FilterationModel> Filters { get; set; }
        public IQueryable<NewRegisteration> Registeration { get; set; }
    }
    public record FilterationModel
    {
        public string SheetId { get; set; }
        public int PassingPrecent { get; set; }
        public Community Community { get; set; }
    }

    internal class DisplayOnCustomerFilterQueryHandler : IRequestHandler<GetOnCustomerFilterQuery, List<GetAllRegisterationQueryDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IOnlineJudgeServices _onlineJudgeServices;

        public DisplayOnCustomerFilterQueryHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IOnlineJudgeServices onlineJudgeServices)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _onlineJudgeServices = onlineJudgeServices;
        }

        public async Task<List<GetAllRegisterationQueryDto>> Handle(GetOnCustomerFilterQuery query, CancellationToken cancellationToken)
        {

            var trainees = await query.Registeration.ToListAsync();

            var sheetsInfo = await GetCodeForceSheetsInfo(query.Filters);

            var acceptableRequests = new List<NewRegisteration>();

            foreach (var trainee in trainees)
            {
                foreach (var sheet in sheetsInfo)
                {
                    var submissions = await _onlineJudgeServices.GetGroupSheetStatusAsync(sheet.SheetId, 300, sheet.Community, trainee.CodeForceHandle)!;
                    if (submissions is null)
                    {
                        continue;
                    }

                    if (IsAcceptedTrainee(submissions, sheet.ProblemCount, sheet.PassingPrecent))
                    {
                        acceptableRequests.Add(trainee);
                    }
                }
            }

            var FilterResult = acceptableRequests.Adapt<List<GetAllRegisterationQueryDto>>(_mapper.Config);

            return FilterResult;
        }

        private async Task<List<SheetInfoDto>> GetCodeForceSheetsInfo(List<FilterationModel> filteration)
        {
            var sheetsInfo = new List<SheetInfoDto>();
            foreach (var filter in filteration)
            {
                var standing = await _onlineJudgeServices.GetGroupSheetStandingAsync(
                    sheetId: filter.SheetId,
                    numberOfRows: 1,
                    unOfficial: true,
                    community: filter.Community)!;

                if (standing is null)
                {
                    continue;
                }

                sheetsInfo.Add(new()
                {
                    SheetId = filter.SheetId,
                    Community = filter.Community,
                    ProblemCount = standing.problems.Count,
                    PassingPrecent = filter.PassingPrecent
                });
            }

            return sheetsInfo;
        }
        private static bool IsAcceptedTrainee(List<CodeForceSubmissionDto> submissions, int sheetProblemCount, int passingPrecent)
        {
            var acceptedProblems = submissions.Where(x => x.verdict == "OK").DistinctBy(x => x.problem.index).Count();

            return Math.Ceiling(acceptedProblems * 100.0 / sheetProblemCount) > passingPrecent;
        }
        public record SheetInfoDto
        {
            public string SheetId { get; set; }
            public int ProblemCount { get; set; }
            public Community Community { get; set; }
            public int PassingPrecent { get; set; }
        }
    }
}
