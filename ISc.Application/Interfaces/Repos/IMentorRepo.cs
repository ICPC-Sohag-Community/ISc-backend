using ISc.Domain.Comman.Dtos;
using ISc.Domain.Models;
using ISc.Domain.Models.CommunityStaff;
using ISc.Domain.Models.IdentityModels;

namespace ISc.Application.Interfaces.Repos
{
    public interface IMentorRepo:IActorQeueryRepo<Mentor>
    {
        Task Delete(Account account,Mentor mentor, int? campId = null);
    }
}
