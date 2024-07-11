using ISc.Application.Interfaces;
using ISc.Presistance;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISC.UnitTests
{
    public class InMemoryDbcontext:ICPCDbContext
    {
        public InMemoryDbcontext(DbContextOptions options):base(options)
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
