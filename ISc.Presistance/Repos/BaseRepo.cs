using ISc.Application.Interfaces.Repos;
using ISc.Domain.Models;
using Microsoft.EntityFrameworkCore;

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
            await _context.SaveChangesAsync();
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
            await _context.SaveChangesAsync();
        }

        public virtual void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

    }
}
