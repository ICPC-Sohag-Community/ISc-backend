using System.Text.Json;
using ISc.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ISc.Infrastructure.Services.ApiRequest
{
    internal class ApiReqeustService : IApiRequestsServices
    {
        private readonly ILogger<ApiReqeustService> _logger;
        public HttpClient HttpClient { get; set; }
        public ApiReqeustService(
            ILogger<ApiReqeustService> logger)
        {
            HttpClient = new HttpClient();
            _logger = logger;
        }

        public async Task<T> GetAsync<T>(string request,string serviceName)
        {
            var response = await HttpClient.GetAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogCritical("request url: " + request + "\n" +
                    await response.RequestMessage!.Content!.ReadAsStringAsync() ?? $"{serviceName} Site Error");
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
