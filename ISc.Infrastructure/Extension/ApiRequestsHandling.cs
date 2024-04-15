using Microsoft.IdentityModel.Tokens;

namespace ISc.Infrastructure.Extension
{
    public static class ApiRequestsHandling
    {
        public static string CreateUri(this Dictionary<string, string> queryParams, string? controller = null)
        {
            queryParams = queryParams.OrderBy(x => x.Key).ToDictionary();

            return (controller.IsNullOrEmpty() ? "" : $"{controller}?")
                + string.Join("&", queryParams.Select(kvp => $"{kvp.Key}={kvp.Value}"));
        }
    }
}
