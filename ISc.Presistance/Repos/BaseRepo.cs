using ISc.Application.Interfaces.Repos;
using ISc.Domain.Models;

namespace ISc.Presistance.Repos
{
    public class BaseRepo<T> : IBaseRepo<T> where T : class
    {
        private readonly ICPCDbContext _context;

        public BaseRepo(ICPCDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(T entity)
        {
           await  _context.AddAsync(entity);
           await  _context.SaveChangesAsync(); 
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return  _context.Set<T>().Select(i=>i);   
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task UpdateAsync(T entity)
        {
            _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
