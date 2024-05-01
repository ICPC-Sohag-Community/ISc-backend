using ISc.Domain.Models;

namespace ISc.Application.Interfaces.Repos
{
    public interface IBaseRepo<T> where T : class
    {
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        void Delete(T entity);
        Task<T> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        IQueryable<T> Entities { get; }
        void DetachedetachedEntity<T>(T entity);
    }
}
