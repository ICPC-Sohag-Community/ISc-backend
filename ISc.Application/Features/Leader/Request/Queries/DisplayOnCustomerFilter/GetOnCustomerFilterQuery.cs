using ISc.Application.Dtos.CodeForce;
using ISc.Application.Features.Leader.Request.Queries.DisplayOnSystemFilter;
using ISc.Application.Interfaces;
using ISc.Application.Interfaces.Repos;
using ISc.Domain.Comman.Enums;
using ISc.Domain.Models;
using ISc.Shared;
using Mapster;
using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ISc.Application.Features.Leader.Request.Queries.DisplayOnCustomerFilter
{
    public record GetOnCustomerFilterQuery : IRequest<Response>
    {
        public int CampId { get; set; }
        public List<FilterationModel> Filters { get; set; }
    }
    public record FilterationModel
    {
        public string SheetId { get; set; }
        public int PassingPrecent { get; set; }
        public Community Community { get; set; }
    }

    internal class DisplayOnCustomerFilterQueryHandler : IRequestHandler<GetOnCustomerFilterQuery, Response>
    {
        private readonly IMediator _mediator;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IOnlineJudgeServices _onlineJudgeServices;

        public DisplayOnCustomerFilterQueryHandler(
            IMediator mediator,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IOnlineJudgeServices onlineJudgeServices)
        {
            _mediator = mediator;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _onlineJudgeServices = onlineJudgeServices;
        }

        public async Task<Response> Handle(GetOnCustomerFilterQuery query, CancellationToken cancellationToken)
        {
            var camp = await _unitOfWork.Repository<Camp>().GetByIdAsync(query.CampId);

            if (camp == null)
            {
                return await Response.FailureAsync("Request not found.", System.Net.HttpStatusCode.NotFound);
            }

            var validTraineesResponse = await _mediator.Send(new GetRegisterationOnSystemFilterQuery(camp.Id));

            var trainees = validTraineesResponse.Data as List<GetRegisterationOnSystemFilterQueryDto>;

            var sheetsInfo = await GetCodeForceSheetsInfo(query.Filters);

            var acceptableRequests = new List<int>();

            foreach (var trainee in trainees)
            {
                foreach (var sheet in sheetsInfo)
                {
                    var submissions = await _onlineJudgeServices.GetGroupSheetStatusAsync(sheet.SheetId, 300, sheet.Community, trainee.CodeForceHandle)!;
                    if(submissions is null)
                    {
                        continue;
                    }

                    if (IsAcceptedTrainee(submissions, sheet.ProblemCount, sheet.PassingPrecent))
                    {
                        acceptableRequests.Add(trainee.Id);
                    }
                }
            }

            var FilterResult = await _unitOfWork.Repository<NewRegisteration>().Entities
                            .Where(x => acceptableRequests.Contains(x.Id))
                            .ProjectToType<GetOnCustomerFilterQueryDto>(_mapper.Config)
                            .ToListAsync();

            return await Response.SuccessAsync(FilterResult);
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
                    ProblemCount= standing.problems.Count,
                    PassingPrecent=filter.PassingPrecent
                });
            }

            return sheetsInfo;
        }
        private static bool IsAcceptedTrainee(List<CodeForceSubmissionDto>submissions,int sheetProblemCount,int passingPrecent)
        {
            var acceptedProblems = submissions.Where(x => x.verdict == "OK").DistinctBy(x => x.problem.index).Count();

            return Math.Ceiling((acceptedProblems * 100.0) / sheetProblemCount) > passingPrecent;
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
