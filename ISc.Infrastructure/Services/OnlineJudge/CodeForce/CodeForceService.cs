using ISc.Application.Dtos.CodeForce;
using ISc.Application.Interfaces;
using ISc.Domain.Comman.Enums;
using ISc.Infrastructure.Extension;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ISc.Infrastructure.Services.OnlineJudge.CodeForce
{
    public class CodeForceService : IOnlineJudgeServices
    {
        private readonly IApiRequestsServices _apiService;
        private Dictionary<string, string> _request;
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;
        public CodeForceService(
            IApiRequestsServices apiRequestsServices,
            IConfiguration configuration,
            ILogger logger)
        {
            _apiService = apiRequestsServices;

            _apiService.HttpClient.BaseAddress = new Uri(configuration["CodeForceBaseUrl"]!.ToString());

            _apiService.HttpClient.DefaultRequestHeaders
                .Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            _request = [];
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<List<CodeForceUserDto>>? GetUsersAsync(List<string> handles)
        {
            var handlesFormat = string.Join(";", handles);

            _request.Add("handles", handlesFormat);

            var response = await _apiService
                .GetAsync<CodeForceBaseResponseDto<List<CodeForceUserDto>>>(_request.CreateUri("user.info"));

            return response.IsSuccess ? response.result : null!;
        }

        public async Task<List<CodeForceStandingDto>>? 
            GetGroupSheetStandingAsync(string sheetId, int numberOfRows, bool unOfficial, Community community)
        {
            var sheetConfig = CodeForceHandlingRequest.GetSheetKeysFactory(_configuration, community, _logger);
            var controller = "contest.status";

            _request = new Dictionary<string, string>
            {
                {"apiKey",sheetConfig.Key },
                {"contestId", sheetId },
                {"count", Math.Max(numberOfRows, 1).ToString() },
                {"from", "1" },
                {"showUnofficial", unOfficial? "true":"false" },
                {"time", CodeForceHandlingRequest.GenerateTimeInUnix()}
            };

            var queryString = _request.CreateUri(controller);
            _request.Add("apiSig", CodeForceHandlingRequest.GenerateSig(queryString, sheetConfig.Value));

            var response = await _apiService
                .GetAsync<CodeForceBaseResponseDto<List<CodeForceStandingDto>>>(_request.CreateUri(controller));

            return response.IsSuccess ? response.result : null!;
        }

        public Task<string> GetGroupSheetStatusAsync()
        {
            throw new NotImplementedException();
        }

        public Task<string> GetUserStatusAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ValidateHandleAsync(string handle)
        {
            _request = new Dictionary<string, string>()
            {
                {"handles",handle }
            };

            var response = await _apiService.
                GetAsync<CodeForceBaseResponseDto<List<CodeForceUserDto>>>(_request.CreateUri("user.info"));

            return response.IsSuccess;
        }

    }
}
