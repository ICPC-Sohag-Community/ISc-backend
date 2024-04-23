using ISc.Application.Interfaces.Repos;
using ISc.Domain.Models;
using Mapster;
using MapsterMapper;

namespace ISc.Presistance.Repos
{
    public class StuffArhcive : BaseRepo<StuffArchive>,IStuffArchiveRepo
    {
        private readonly ICPCDbContext _context;
        public StuffArhcive(
            ICPCDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task AddToArchiveAsync(Stuff member)
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

        private StuffArchive? Found(Stuff member)
        {
            return _context.StuffArchives.SingleOrDefault(x =>
            (x.NationalId == member.NationalId) ||
            (x.PhoneNumber == member.PhoneNumber) ||
            ((x.FirstName + x.MiddelName + x.LastName).ToLower() == (member.FirstName + x.MiddelName + x.LastName).ToLower()));
        }
    }
}
