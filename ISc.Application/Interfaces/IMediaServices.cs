using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace ISc.Application.Interfaces
{
    public interface IMediaServices
    {
        Task<string>? SaveAsync(IFormFile? media);

        Task DeleteAsync(string url);

        Task<string> UpdateAsync(string oldUrl, IFormFile newMedia);

        string GetUrl(string url);
    }
}
