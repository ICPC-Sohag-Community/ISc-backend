using ISc.Domain.Comman.Dtos;
using ISc.Domain.Models.CommunityStaff;
using ISc.Domain.Models.IdentityModels;

namespace ISc.Application.Interfaces.Repos
{
    public interface IHeadRepo:IActorQeueryRepo<HeadOfCamp>
    {
        Task Delete(Account account,HeadOfCamp head);
    }
}
