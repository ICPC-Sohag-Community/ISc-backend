using ISc.Domain.Models;
using ISc.Domain.Models.IdentityModels;

namespace ISc.Application.Interfaces.Repos
{
    public interface IStuffArchiveRepo : IBaseRepo<StuffArchive>
    {
        Task AddToArchiveAsync(Account member,string role);
    }
}
