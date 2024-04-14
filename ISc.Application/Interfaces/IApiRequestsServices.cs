using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;

namespace ISc.Application.Interfaces
{
    public interface IApiRequestsServices
    {
        public Task<T> GetAsync<T>(string request);
        public Task<string> PostAsync();
    }
}
