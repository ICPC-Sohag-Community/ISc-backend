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
        Task<CodeForceStandingDto>? GetGroupSheetStandingAsync(
            string sheetId, int numberOfRows, bool unOfficial, Community community);
        Task<List<CodeForceSubmissionDto>>? GetGroupSheetStatusAsync(
            string sheetId, int count, Community community = Community.Sohag, string? handle = null);
        Task<List<CodeForceSubmissionDto>>? GetUserStatusAsync(string handle, Community community);
        Task<List<CodeForceUserDto>>? GetUsersAsync(List<string> handles);
    }
}
