using System.Collections.Immutable;
using ISc.Application.Extension;
using ISc.Application.Interfaces.Repos;
using ISc.Domain.Comman.Constant;
using ISc.Domain.Comman.Dtos;
using ISc.Domain.Models;
using ISc.Domain.Models.IdentityModels;
using ISc.Shared.Exceptions;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ISc.Presistance.Repos
{
    public class TraineeRepo : ITraineeRepo
    {
        private readonly ICPCDbContext _context;
        private readonly UserManager<Account> _userManager;


        public TraineeRepo(
            ICPCDbContext context,
            UserManager<Account> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public IQueryable<Trainee> Entities => _context.Trainees;

        public async void Delete(Account entity, bool isComplete)
        {
            var trainee = await _context.Trainees.FindAsync(entity.Id);

            if (trainee is null)
            {
                throw new BadRequestException("Invalid request.");
            }

            var rolesCount = _userManager.GetRolesAsync(entity).Result.Count;

            await AddToArchive(entity, trainee, isComplete);

            if (rolesCount == 1)
            {
                await _userManager.DeleteAsync(entity);
            }
            else
            {
                _context.TraineeAttendences.RemoveRange(_context.TraineeAttendences.Where(x => x.TraineeId == trainee.Id));
                _context.TraineeTasks.RemoveRange(_context.TraineeTasks.Where(x => x.TraineeId == trainee.Id));
                _context.TraineesAccesses.RemoveRange(_context.TraineesAccesses.Where(x => x.TraineeId == trainee.Id));
                _context.SessionFeedbacks.RemoveRange(_context.SessionFeedbacks.Where(x => x.TraineeId == trainee.Id));

                await _userManager.RemoveFromRoleAsync(entity, Roles.Trainee);
                _context.Remove(trainee);
            }
        }
        public async Task UpdateAsync(AccountModel<Trainee> entity)
        {
            if (entity.Account != null)
            {
                await _userManager.DeleteAsync(entity.Account);
            }

            if (entity.Member != null)
            {
                _context.Update(entity.Member);
            }
        }
        public async Task AddAsync(AccountModel<Trainee> entity)
        {
            if (entity.Account != null && entity.Password != null)
            {
                await _userManager.CreateAsync(entity.Account, entity.Password);
            }
            await _context.AddAsync(entity.Member);
        }
        private async Task AddToArchive(Account account, Trainee entity, bool isComplete)
        {
            var campName = entity.Camp.Name;

            var archive = await FoundAsync(account, campName);

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

        private async Task<TraineeArchive?> FoundAsync(Account entity, string campName)
        {
            return await _context.TraineesArchives.SingleOrDefaultAsync(x =>
                        (x.NationalId == entity.NationalId ||
                        x.PhoneNumber == entity.PhoneNumber ||
                        (x.FirstName + x.MiddleName + x.LastName).ToLower() == (entity.FirstName + entity.MiddleName + entity.LastName).ToLower())
                        && campName.ToLower().Trim() == x.CampName.ToLower().Trim());
        }

        public async Task<Trainee?> GetByIdAsync(string id)
        {
            return await _context.Trainees.FindAsync(id);
        }

        public async Task<IEnumerable<Trainee>?> GetAllAsync()
        {
            return await _context.Trainees.ToListAsync();
        }

        public async Task AddRangeAsync(ICollection<Trainee> entities)
        {
            await _context.AddRangeAsync(entities);
        }
    }
}
