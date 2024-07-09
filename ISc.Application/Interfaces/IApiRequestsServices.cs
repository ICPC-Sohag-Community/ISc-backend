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
        HttpClient HttpClient { get; set; }
         Task<T> GetAsync<T>(string request,string serviceName);
         Task<string> PostAsync();
    }
}
