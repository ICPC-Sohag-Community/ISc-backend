﻿using ISc.Application.Dtos.Standing;
using ISc.Application.Interfaces;
using ISc.Application.Interfaces.Repos;
using ISc.Domain.Models.IdentityModels;
using MapsterMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections;

namespace ISc.Presistance.Repos
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ICPCDbContext _context;
        private Hashtable _repositories;

        public UnitOfWork(
            ICPCDbContext context,
            UserManager<Account> userManager,
            IStuffArchiveRepo stuffRepo,
            IMediaServices mediaServices,
            IMapper mapper)
        {
            _context = context;
            _repositories = new Hashtable();

            Mentors = new MentorRepo(context, userManager, stuffRepo, mediaServices);
            Trainees = new TraineeRepo(context, userManager, mediaServices, mapper);
            Heads = new HeadRepo(context, userManager, stuffRepo, mediaServices);
        }

        public IMentorRepo Mentors { get; private set; }

        public ITraineeRepo Trainees { get; private set; }

        public IHeadRepo Heads { get; private set; }

        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task<ICollection<StandingDto>> GetStandingAsync(int campId)
        {
            return await _context.Trainees
                        .Where(x => x.CampId == campId)
                        .Select(x => new StandingDto()
                        {
                            Id = x.Id,
                            FirstName = x.Account.FirstName,
                            MiddleName = x.Account.MiddleName,
                            LastName = x.Account.LastName,
                            CodeForceHandle = x.Account.CodeForceHandle
                        }).AsNoTracking()
                        .ToListAsync();
        }

        IBaseRepo<T> IUnitOfWork.Repository<T>()
        {
            if (_repositories == null)
                _repositories = new Hashtable();

            var type = typeof(T).Name;

            if (!_repositories.ContainsKey(type))
            {
                var repositoryType = typeof(BaseRepo<>);

                var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(T)), _context);

                _repositories.Add(type, repositoryInstance);
            }

            return (IBaseRepo<T>)_repositories[type];
        }

    }
}
