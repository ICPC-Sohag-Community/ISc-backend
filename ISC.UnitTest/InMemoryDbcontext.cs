using ISc.Presistance;
using Microsoft.EntityFrameworkCore;

namespace ISC.UnitTests
{
    public class InMemoryDbcontext : ICPCDbContext
    {
        public InMemoryDbcontext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());
        }

        public override void Dispose()
        {
            Database.EnsureDeleted();
            base.Dispose();
        }
    }
}
