using ISc.Domain.Comman.Dtos;
using ISc.Domain.Models.CommunityStuff;
using ISc.Domain.Models.IdentityModels;

namespace ISc.Application.Interfaces.Repos
{
    public interface IHeadRepo:IActorQeueryRepo<HeadOfCamp>
    {
        void Delete(Account entity);
    }
}
