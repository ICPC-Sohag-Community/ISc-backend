using ISc.Application.Interfaces.Repos;

namespace ISc.Presistance.Repos
{
    public class UnitOfWork : IUnitOFWork
    {
        private readonly ICPCDbContext _context;

        public UnitOfWork(ICPCDbContext context)
        {
            _context = context;
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();   
        }
    }
}
