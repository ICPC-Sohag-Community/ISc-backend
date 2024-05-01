using ISc.Application.Interfaces.Repos;
using ISc.Domain.Comman.Constant;
using ISc.Domain.Comman.Dtos;
using ISc.Domain.Models;
using ISc.Domain.Models.CommunityStuff;
using ISc.Domain.Models.IdentityModels;
using ISc.Shared.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ISc.Presistance.Repos
{
    public class HeadRepo : IHeadRepo
    {
        private readonly UserManager<Account> _userManager;
        private readonly ICPCDbContext _context;
        private readonly IStuffArchiveRepo _archiveRepo;

        public IQueryable<HeadOfCamp> Entities => _context.Set<HeadOfCamp>();

        public HeadRepo(
            ICPCDbContext context,
            UserManager<Account> userManager,
            IStuffArchiveRepo archiveRepo)
        {
            _context = context;
            _userManager = userManager;
            _archiveRepo = archiveRepo;
        }

        public async void Delete(Account entity)
        {
            var head = await _context.HeadsOfCamps.FindAsync(entity.Id);

            if (head is null)
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
                await _userManager.RemoveFromRoleAsync(entity, Roles.Head_Of_Camp);
                _context.HeadsOfCamps.Remove(head);
            }
        }

        public async Task AddAsync(AccountModel<HeadOfCamp> entity)
        {
            if (entity.Account is not null && entity.Password is not null)
            {
                await _userManager.CreateAsync(entity.Account, entity.Password);
            }
            await _context.HeadsOfCamps.AddAsync(entity.Member);
        }

        public async Task UpdateAsync(AccountModel<HeadOfCamp> entity)
        {
            if(entity.Account != null)
            {
                await _userManager.UpdateAsync(entity.Account);

            }

            if (entity.Member != null)
            {
                _context.Update(entity.Member);
            }
        }

        public async Task<HeadOfCamp?> GetByIdAsync(string id)
        {
            return await _context.HeadsOfCamps.FindAsync(id);
        }
        public async Task<IEnumerable<HeadOfCamp>?> GetAllAsync()
        {
            return await _context.HeadsOfCamps.ToListAsync();
        }
    }
}
