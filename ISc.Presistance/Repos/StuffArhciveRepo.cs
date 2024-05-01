using ISc.Application.Interfaces.Repos;
using ISc.Domain.Models;
using ISc.Domain.Models.IdentityModels;
using Mapster;
using MapsterMapper;

namespace ISc.Presistance.Repos
{
    public class StuffArhciveRepo : BaseRepo<StuffArchive>,IStuffArchiveRepo
    {
        private readonly ICPCDbContext _context;
        public StuffArhciveRepo(
            ICPCDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task AddToArchiveAsync(Account member)
        {
            var entity = Found(member);

            if (entity is null)
            {
                await _context.AddAsync(member.Adapt<StuffArchive>());
            }
            else
            {
                _context.Update(member.Adapt<StuffArchive>());
            } 
        }

        private StuffArchive? Found(Account member)
        {
            return _context.StuffArchives.SingleOrDefault(x =>
            (x.NationalId == member.NationalId) ||
            (x.PhoneNumber == member.PhoneNumber) ||
            ((x.FirstName + x.MiddelName + x.LastName).ToLower() == (member.FirstName + x.MiddelName + x.LastName).ToLower()));
        }
    }
}
