using ISc.Application.Interfaces.Repos;
using ISc.Domain.Comman.Constant;
using ISc.Domain.Comman.Dtos;
using ISc.Domain.Models.CommunityStuff;
using ISc.Domain.Models.IdentityModels;
using ISc.Shared.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ISc.Presistance.Repos
{
    public class MentorRepo : IMentorRepo
    {
        private readonly ICPCDbContext _context;
        private readonly UserManager<Account> _userManager;
        private readonly IStuffArchiveRepo _archiveRepo;


        public MentorRepo(
            ICPCDbContext context,
            UserManager<Account> userManager,
            IStuffArchiveRepo archiveRepo)
        {
            _context = context;
            _userManager = userManager;
            _archiveRepo = archiveRepo;
        }
        
        public IQueryable<Mentor> Entities => _context.Set<Mentor>();

        public async void Delete(Account entity)
        {
            var mentor = await _context.Mentors.FindAsync(entity.Id);

            if (mentor is null)
            {
                throw new BadRequestException("Invalid request.");
            }
            var rolesCount = _userManager.GetRolesAsync(entity).Result.Count;

            await _archiveRepo.AddToArchiveAsync(entity);

            if (rolesCount == 1)
            {
                await _userManager.DeleteAsync(entity);
            }
            else
            {
                DeleteMentorTrainees(mentor!);
                _context.MentorsOfCamps.RemoveRange(_context.MentorsOfCamps.Where(p => p.MentorId == entity.Id));
                await _userManager.RemoveFromRoleAsync(entity, Roles.Mentor);
                _context.Mentors.Remove(mentor);
            }
        }
        public async Task UpdateAsync(AccountModel<Mentor> entity)
        {
            if (entity.Account != null)
            {
                await _userManager.UpdateAsync(entity.Account);

            }

            if (entity.Member != null)
            {
                _context.Update(entity.Member);
            }
        }
        public async Task AddAsync(AccountModel<Mentor> entity)
        {
            if (entity.Account is not null && entity.Password is not null)
            {
                await _userManager.CreateAsync(entity.Account, entity.Password);
            }
            await _context.AddAsync(entity.Member);
        }
        public async Task<Mentor?> GetByIdAsync(string id)
        {
            return await _context.Set<Mentor>().FindAsync(id);
        }
        public async Task<IEnumerable<Mentor>?> GetAllAsync()
        {
            return await _context.Mentors.ToListAsync();
        }

        private async void DeleteMentorTrainees(Mentor entity)
        {
            var trainees = _context.Trainees.Where(p => p.MentorId == entity.Id);
            await trainees.ForEachAsync(x => x.MentorId = null);
        }

        public async Task AddRangeAsync(ICollection<Mentor> entities)
        {
            await _context.AddRangeAsync(entities);
        }
    }
}
