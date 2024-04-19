using ISc.Domain.Interface;

namespace ISc.Application.Interfaces.Repos
{
    public interface IUnitOfWork:IDisposable
    {

        Task<int> SaveAsync();
        IBaseRepo<T> Repository<T>() where T : class, ISoftEntity;
    }
}
