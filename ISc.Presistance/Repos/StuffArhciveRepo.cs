using ISc.Application.Interfaces.Repos;
using ISc.Domain.Models;
using ISc.Domain.Models.IdentityModels;
using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace ISc.Presistance.Repos
{
    public class StuffArhciveRepo : BaseRepo<StuffArchive>,IStuffArchiveRepo
    {
        private readonly ICPCDbContext _context;
        private readonly IMapper _mapper;
        public StuffArhciveRepo(
            ICPCDbContext context,
            IMapper mapper) : base(context)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task AddToArchiveAsync(Account member,string role)
        {
            var entity = await Found(member, role);

            if (entity is null)
            {
                await _context.AddAsync(member.Adapt<StuffArchive>());
            }
            else
            {
                _mapper.Map(member, entity);
                _context.Update(entity);
            } 
        }

        private async Task<StuffArchive?> Found(Account member,string role)
        {
            return await _context.StuffArchives.SingleOrDefaultAsync(x =>
            ((x.NationalId == member.NationalId) ||
            (x.PhoneNumber == member.PhoneNumber) ||
            ((x.FirstName + x.MiddleName + x.LastName).ToLower() == (member.FirstName + x.MiddleName + x.LastName).ToLower())) && x.Role == role);
        }
    }
}
