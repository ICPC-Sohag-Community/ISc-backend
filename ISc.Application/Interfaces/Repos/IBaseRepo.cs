using ISc.Application.Dtos.CodeForce;
using ISc.Application.Dtos.Standing;
using ISc.Domain.Models;

namespace ISc.Application.Interfaces.Repos
{
    public interface IBaseRepo<T> where T : class
    {
        Task AddAsync(T entity);
        Task AddRangeAsync(ICollection<T> entities);
        Task UpdateAsync(T entity);
        void UpdateRange(ICollection<T> entities);
        void Delete(T entity);
        void DeleteRange(ICollection<T> entities);
        Task<T?> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        IQueryable<T> Entities { get; }
        void DetachedetachedEntity<T>(T entity);
    }
}
