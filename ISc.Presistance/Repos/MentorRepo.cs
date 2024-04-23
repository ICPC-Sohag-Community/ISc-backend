using ISc.Application.Interfaces.Repos;
using ISc.Domain.Comman.Constant;
using ISc.Domain.Models.CommunityStuff;
using ISc.Domain.Models.IdentityModels;
using Microsoft.AspNetCore.Identity;

namespace ISc.Presistance.Repos
{
    public class MentorRepo : BaseRepo<Mentor>, IMentorRepo
    {
        private readonly ICPCDbContext _context;
        private readonly UserManager<Account> _userManager;
        private readonly IStuffArchiveRepo _archiveRepo;
        public MentorRepo(
            ICPCDbContext context,
            UserManager<Account> userManager,
            IStuffArchiveRepo archiveRepo) : base(context)
        {
            _context = context;
            _userManager = userManager;
            _archiveRepo = archiveRepo;
        }
        public async override void Delete(Mentor entity)
        {
            var rolesCount = _userManager.GetRolesAsync(entity).Result.Count();

            await _archiveRepo.AddToArchiveAsync(entity);

            if (rolesCount == 1)
            {
                await _userManager.DeleteAsync(entity);
            }
            else
            {
                DeleteMentorTrainees(entity);
                _context.MentorsOfCamps.RemoveRange(_context.MentorsOfCamps.Where(p => p.MentorId == entity.Id));
                await _userManager.RemoveFromRoleAsync(entity, Roles.Mentor);
                _context.Remove(entity);
            }
        }
        public override Task UpdateAsync(Mentor entity)
        {
            return _userManager.UpdateAsync(entity);
        }
        public override Task AddAsync(Mentor entity)
        {
            return _userManager.CreateAsync(entity);
        }
        private void DeleteMentorTrainees(Mentor entity)
        {
            var trainees = _context.Trainees.Where(p => p.MentorId == entity.Id);
            foreach (var trainee in trainees)
            {

                trainee.MentorId = null;
            }
        }

    }
}
