using ISc.Domain.Models;

namespace ISc.Application.Interfaces.Repos
{
    public interface IStuffArchiveRepo : IBaseRepo<StuffArchive>
    {
        Task AddToArchiveAsync(Stuff member);
    }
}
