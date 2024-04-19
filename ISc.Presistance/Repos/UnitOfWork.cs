using System.Collections;
using ISc.Application.Interfaces.Repos;
using Microsoft.EntityFrameworkCore;

namespace ISc.Presistance.Repos
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ICPCDbContext _context;
        private Hashtable _repositories;

        public UnitOfWork(ICPCDbContext context)
        {
            _context = context;
            _repositories = new Hashtable();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();   
        }

        IBaseRepo<T> IUnitOfWork.Repository<T>()
        {
            if (_repositories == null)
                _repositories = new Hashtable();

            var type = typeof(T).Name;

            if (!_repositories.ContainsKey(type))
            {
                var repositoryType = typeof(BaseRepo<>);

                var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(T)), _context);

                _repositories.Add(type, repositoryInstance);
            }

            return (IBaseRepo<T>)_repositories[type];
        }
        
    }
}
