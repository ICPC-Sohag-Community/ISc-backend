using Microsoft.AspNetCore.Http;

namespace ISc.Application.Interfaces
{
    public interface IMediaServices
    {
        Task<string> SaveAsync(IFormFile? media);

        Task DeleteAsync(string url);

        Task<string> UpdateAsync(string oldUrl, IFormFile newMedia);

        string GetUrl(string url);
    }
}
