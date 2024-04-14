using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISc.Application.Interfaces
{
    public interface IOnlineJudgeServices
    {
        Task<bool> ValidateHandleAsync(string handle);
        Task<string> GetContestStandingAsync();
        Task<string> GetContestStatusAsync();
        Task<string> GetUserStatusAsync();
    }
}
