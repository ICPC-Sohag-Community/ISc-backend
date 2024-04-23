using ISc.Application.Interfaces.Repos;
using ISc.Domain.Comman.Constant;
using ISc.Domain.Models;
using ISc.Domain.Models.IdentityModels;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ISc.Presistance.Repos
{
    public class TraineeRepo : BaseRepo<Trainee>, ITraineeRepo
    {
        private readonly ICPCDbContext _context;
        private readonly UserManager<Account> _userManager;
        public TraineeRepo(
            ICPCDbContext context,
            UserManager<Account> userManager) : base(context)
        {
            _context = context;
            _userManager = userManager;
        }
        public async void Delete(Trainee entity, bool isComplete)
        {
            var rolesCount = _userManager.GetRolesAsync(entity).Result.Count;

            if (rolesCount == 1)
            {
                base.Delete(entity);
            }
            else
            {
                _context.TraineeAttendences.RemoveRange(_context.TraineeAttendences.Where(x => x.TraineeId == entity.Id));
                _context.TraineeTasks.RemoveRange(_context.TraineeTasks.Where(x => x.TraineeId == entity.Id));
                _context.TraineesAccesses.RemoveRange(_context.TraineesAccesses.Where(x => x.TraineeId == entity.Id));
                _context.SessionFeedbacks.RemoveRange(_context.SessionFeedbacks.Where(x => x.TraineeId == entity.Id));

                await _userManager.RemoveFromRoleAsync(entity, Roles.Trainee);
            }
            await AddToArchive(entity, isComplete);
        }

        private async Task AddToArchive(Trainee entity, bool isComplete)
        {
            var campName = entity.Camp.Name;

            var archive = await FoundAsync(entity, campName);

            var archiveWasNull = archive is null;

            archive = entity.Adapt<TraineeArchive>();
            archive.IsComplete = isComplete;
            archive.Year = DateTime.Now.Year;
            archive.CampName = campName;

            if (archiveWasNull)
            {
                await _context.TraineesArchives.AddAsync(archive);
            }
            else
            {
                _context.TraineesArchives.Update(archive);
            }
        }

        private async Task<TraineeArchive?> FoundAsync(Trainee entity, string campName)
        {
            return await _context.TraineesArchives.SingleOrDefaultAsync(x =>
                        (x.NationalId == entity.NationalId ||
                        x.PhoneNumber == entity.PhoneNumber ||
                        (x.FirstName + x.MiddelName + x.LastName).ToLower() == (entity.FirstName + entity.MiddleName + entity.LastName).ToLower())
                        && entity.Camp.Name.ToLower().Trim() == x.CampName.ToLower().Trim());
        }
    }
}
