using ISc.Domain.Models.IdentityModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ISc.Presistance
{
    public class ICPCDbContext : IdentityDbContext<Account>
    {
        public ICPCDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}
