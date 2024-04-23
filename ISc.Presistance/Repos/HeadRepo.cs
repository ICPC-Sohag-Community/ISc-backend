using ISc.Application.Interfaces.Repos;
using ISc.Domain.Comman.Constant;
using ISc.Domain.Models.CommunityStuff;
using ISc.Domain.Models.IdentityModels;
using Microsoft.AspNetCore.Identity;

namespace ISc.Presistance.Repos
{
    public class HeadRepo : BaseRepo<HeadOfCamp>, IHeadRepo
    {
        private readonly UserManager<Account> _userManager;
        private readonly ICPCDbContext _context;
        private readonly IStuffArchiveRepo _archiveRepo;
        public HeadRepo(
            ICPCDbContext context,
            UserManager<Account> userManager,
            IStuffArchiveRepo archiveRepo) : base(context)
        {
            _context = context;
            _userManager = userManager;
            _archiveRepo = archiveRepo;
        }

        public async override void Delete(HeadOfCamp entity)
        {
            var rolesCount = _userManager.GetRolesAsync(entity).Result.Count;

            await _archiveRepo.AddToArchiveAsync(entity);

            if (rolesCount == 1)
            {
                await _userManager.DeleteAsync(entity);
            }
            else
            {
                await _userManager.RemoveFromRoleAsync(entity, Roles.Head_Of_Camp);
                _context.Remove(entity);
            }
        }

        public override Task AddAsync(HeadOfCamp entity)
        {
            return _userManager.CreateAsync(entity);
        }

        public override Task UpdateAsync(HeadOfCamp entity)
        {
            return _userManager.UpdateAsync(entity);
        }
    }
}
