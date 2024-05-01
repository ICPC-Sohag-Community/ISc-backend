using ISc.Application.Interfaces.Repos;
using ISc.Domain.Models;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ISc.Presistance.Repos
{
    public class BaseRepo<T> : IBaseRepo<T> where T : class
    {
        private readonly ICPCDbContext _context;

        public BaseRepo(ICPCDbContext context)
        {
            _context = context;
        }

        public virtual async Task AddAsync(T entity)
        {
            await _context.AddAsync(entity);
        }

        public IQueryable<T> Entities => _context.Set<T>();

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>().Select(i => i).ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public virtual async Task UpdateAsync(T entity)
        {
            _context.Set<T>().Update(entity);
        }

        public virtual void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public void DetachedetachedEntity<T>(T entity)
        {
            _context.Entry(entity).State = EntityState.Detached;
        }
    }
}
