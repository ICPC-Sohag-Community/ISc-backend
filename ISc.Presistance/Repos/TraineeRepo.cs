using System.Collections.Immutable;
using ISc.Application.Interfaces;
using ISc.Application.Interfaces.Repos;
using ISc.Domain.Comman.Constant;
using ISc.Domain.Comman.Dtos;
using ISc.Domain.Models;
using ISc.Domain.Models.IdentityModels;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ISc.Presistance.Repos
{
    public class TraineeRepo : ITraineeRepo
    {
        private readonly ICPCDbContext _context;
        private readonly UserManager<Account> _userManager;
        private readonly IMediaServices _mediaServices;


        public TraineeRepo(
            ICPCDbContext context,
            UserManager<Account> userManager,
            IMediaServices mediaServices)
        {
            _context = context;
            _userManager = userManager;
            _mediaServices = mediaServices;
        }
        public IQueryable<Trainee> Entities => _context.Trainees;

        public async Task Delete(Account account,Trainee trainee, bool isComplete)
        {

            var rolesCount = _userManager.GetRolesAsync(account).Result.Count;

            await AddToArchive(account, trainee, isComplete);

            if (rolesCount == 1)
            {
                if(account.PhotoUrl is not null)
                {
                    await _mediaServices.DeleteAsync(account.PhotoUrl);
                }

                await _userManager.DeleteAsync(account);
            }
            else
            {
                _context.TraineeAttendences.RemoveRange(_context.TraineeAttendences.Where(x => x.TraineeId == trainee.Id));
                _context.TraineeTasks.RemoveRange(_context.TraineeTasks.Where(x => x.TraineeId == trainee.Id));
                _context.TraineesAccesses.RemoveRange(_context.TraineesAccesses.Where(x => x.TraineeId == trainee.Id));
                _context.SessionFeedbacks.RemoveRange(_context.SessionFeedbacks.Where(x => x.TraineeId == trainee.Id));

                await _userManager.RemoveFromRoleAsync(account, Roles.Trainee);
                _context.Remove(trainee);
            }
        }
        public async Task UpdateAsync(AccountModel<Trainee> entity)
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
        public async Task AddAsync(AccountModel<Trainee> entity)
        {
            if (entity.Account != null && entity.Password != null)
            {
                await _userManager.CreateAsync(entity.Account, entity.Password);
                entity.Member.Id = entity.Account.Id;
            }

            await _context.AddAsync(entity.Member);

            if (!await _userManager.IsInRoleAsync(entity.Account!, Roles.Trainee))
            {
                await _userManager.AddToRoleAsync(entity.Account!, Roles.Trainee);
            }
        }
        private async Task AddToArchive(Account account, Trainee entity, bool isComplete)
        {
            var campName = entity.Camp.Name;

            var archive = await FoundAsync(account, campName);
           
            if (archive is null)
            {
                archive = account.Adapt<TraineeArchive>();
                FillArchive(isComplete, campName, archive);

                await _context.TraineesArchives.AddAsync(archive);
            }
            else
            {
                FillArchive(isComplete, campName, archive);

                _context.TraineesArchives.Update(archive);
            }
        }

        private static void FillArchive(bool isComplete, string campName, TraineeArchive archive)
        {
            archive.IsComplete = isComplete;
            archive.Year = DateTime.Now.Year;
            archive.CampName = campName;
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
