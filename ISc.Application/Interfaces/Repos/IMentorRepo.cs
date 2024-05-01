using ISc.Domain.Comman.Dtos;
using ISc.Domain.Models;
using ISc.Domain.Models.CommunityStuff;
using ISc.Domain.Models.IdentityModels;

namespace ISc.Application.Interfaces.Repos
{
    public interface IMentorRepo:IActorQeueryRepo<Mentor>
    {
        void Delete(Account entity);
    }
}
