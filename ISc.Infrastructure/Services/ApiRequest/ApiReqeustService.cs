using System.Text.Json;
using System.Web;
using ISc.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ISc.Infrastructure.Services.ApiRequest
{
    internal class ApiReqeustService : IApiRequestsServices
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ApiReqeustService> _logger;
        public ApiReqeustService(
            IConfiguration configuration,
            ILogger<ApiReqeustService> logger)
        {

            _httpClient = new()
            {
                BaseAddress = new Uri(configuration["CodeForceBaseUrl"]!.ToString())
            };

            _httpClient.DefaultRequestHeaders
                .Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            _logger = logger;
        }

        public async Task<T> GetAsync<T>(string request)
        {
            var response = await _httpClient.GetAsync(request);

            if(!response.IsSuccessStatusCode)
            {
                _logger.LogCritical("request url: " + request + "\n" +
                    await response.RequestMessage?.Content?.ReadAsStringAsync() ?? "CodeForce Site Error");
            }

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(content)!;
        }

        public Task<string> PostAsync()
        {
            throw new NotImplementedException();
        }
    }
}
