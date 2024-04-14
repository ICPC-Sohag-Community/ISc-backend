using ISc.Application.Interfaces;

namespace ISc.Infrastructure.Services.OnlineJudge.CodeForce
{
    public class CodeForceService : IOnlineJudgeServices
    {
        public Task<string> GetContestStandingAsync()
        {
            throw new NotImplementedException();
        }

        public Task<string> GetContestStatusAsync()
        {
            throw new NotImplementedException();
        }

        public Task<string> GetUserStatusAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> ValidateHandleAsync(string handle)
        {
            throw new NotImplementedException();
        }
    }
}
