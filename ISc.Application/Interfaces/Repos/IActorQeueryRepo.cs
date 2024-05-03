using ISc.Domain.Comman.Dtos;
using ISc.Domain.Models;
using ISc.Domain.Models.IdentityModels;

namespace ISc.Application.Interfaces.Repos
{
    public interface IActorQeueryRepo<T> where T : class
    {
        Task UpdateAsync(AccountModel<T> entity);
        Task AddAsync(AccountModel<T> entity);
        Task AddRangeAsync(ICollection<T> entities);
        Task<T?> GetByIdAsync(string id);
        Task<IEnumerable<T>?> GetAllAsync();
        IQueryable<T> Entities { get; }
    }
}
