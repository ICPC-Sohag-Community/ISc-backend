using ISc.Application.Interfaces;
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
        private readonly IMediaServices _mediaServices;

        public IQueryable<HeadOfCamp> Entities => _context.Set<HeadOfCamp>();

        public HeadRepo(
            ICPCDbContext context,
            UserManager<Account> userManager,
            IStuffArchiveRepo archiveRepo,
            IMediaServices mediaServices)
        {
            _context = context;
            _userManager = userManager;
            _archiveRepo = archiveRepo;
            _mediaServices = mediaServices;
        }

        public async Task Delete(Account account,HeadOfCamp head)
        {
            if (head is null)
            {
                throw new BadRequestException("Invalid request.");
            }

            var rolesCount = _userManager.GetRolesAsync(account).Result.Count;

            await _archiveRepo.AddToArchiveAsync(account,Roles.Head_Of_Camp);

            if (rolesCount == 1)
            {
                if (account.PhotoUrl is not null)
                {
                    await _mediaServices.DeleteAsync(account.PhotoUrl);
                }

                await _userManager.DeleteAsync(account);
            }
            else
            {
                await _userManager.RemoveFromRoleAsync(account, Roles.Head_Of_Camp);
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

            if (!await _userManager.IsInRoleAsync(entity.Account!,Roles.Head_Of_Camp))
            {
                await _userManager.AddToRoleAsync(entity.Account!,Roles.Head_Of_Camp);
            }
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

        public async Task AddRangeAsync(ICollection<HeadOfCamp> entities)
        {
            await _context.AddRangeAsync(entities);
        }
    }
}
