using ISc.Application.Interfaces;
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
        private readonly IMediaServices _mediaServices;


        public MentorRepo(
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

        public IQueryable<Mentor> Entities => _context.Set<Mentor>();

        public async Task Delete(Account account, Mentor mentor, int? campId = null)
        {
            if (mentor is null)
            {
                throw new BadRequestException("Invalid request.");
            }

            var rolesCount = _userManager.GetRolesAsync(account).Result.Count;

            var participatedCamps = mentor.Camps.Count;
            if (rolesCount == 1 && participatedCamps == 1)
            {
                if (account.PhotoUrl is not null)
                {
                    await _mediaServices.DeleteAsync(account.PhotoUrl);
                }

                await UnAssignMentorTrainees(mentor!);
                await _archiveRepo.AddToArchiveAsync(account, Roles.Mentor);
                await _userManager.DeleteAsync(account);
            }
            else
            {
                if (campId is not null)
                {
                    await UnAssignMentorTrainees(mentor!, campId);

                    var mentorOfCamp = await _context.MentorsOfCamps.SingleAsync(x => x.CampId == campId && x.MentorId == account.Id);
                    _context.MentorsOfCamps.Remove(mentorOfCamp);
                }
                else
                {
                    await UnAssignMentorTrainees(mentor!);

                    var mentorOfCamps = await _context.MentorsOfCamps.Where(p => p.MentorId == account.Id).ToListAsync();
                    _context.MentorsOfCamps.RemoveRange(mentorOfCamps);
                }

                if (participatedCamps == 1)
                {
                    await _userManager.RemoveFromRoleAsync(account, Roles.Mentor);
                    _context.Mentors.Remove(mentor);
                }
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

            if (!await _userManager.IsInRoleAsync(entity.Account!, Roles.Mentor))
            {
                await _userManager.AddToRoleAsync(entity.Account!, Roles.Mentor);
            }
        }
        public async Task<Mentor?> GetByIdAsync(string id)
        {
            return await _context.Set<Mentor>().FindAsync(id);
        }
        public async Task<IEnumerable<Mentor>?> GetAllAsync()
        {
            return await _context.Mentors.ToListAsync();
        }

        private async Task UnAssignMentorTrainees(Mentor entity, int? campId = null)
        {
            if (campId is not null)
            {
                var trainees = _context.Trainees.Where(p => p.MentorId == entity.Id && p.CampId == campId);
                await trainees.ForEachAsync(x => x.MentorId = null);
            }
            else
            {
                var trainees = _context.Trainees.Where(p => p.MentorId == entity.Id);
                await trainees.ForEachAsync(x => x.MentorId = null);
            }
        }

        public async Task AddRangeAsync(ICollection<Mentor> entities)
        {
            await _context.AddRangeAsync(entities);
        }
    }
}
