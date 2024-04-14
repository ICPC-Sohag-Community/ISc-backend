using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISc.Application.Dtos.CodeForce;
using ISc.Domain.Comman.Enums;

namespace ISc.Application.Interfaces
{
    public interface IOnlineJudgeServices
    {
        Task<bool> ValidateHandleAsync(string handle);
        Task<List<CodeForceStandingDto>>? GetGroupSheetStandingAsync(string sheetId, int numberOfRows, bool unOfficial, Community community);
        Task<string> GetGroupSheetStatusAsync();
        Task<string> GetUserStatusAsync();
        Task<List<CodeForceUserDto>>? GetUsersAsync(List<string> handles);
    }
}
