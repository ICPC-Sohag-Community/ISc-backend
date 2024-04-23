using System.Collections;
using ISc.Application.Interfaces.Repos;
using ISc.Domain.Models.IdentityModels;
using Microsoft.AspNetCore.Identity;

namespace ISc.Presistance.Repos
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ICPCDbContext _context;
        private Hashtable _repositories;

        public UnitOfWork(
            ICPCDbContext context,
            UserManager<Account> userManager,
            IStuffArchiveRepo stuffRepo)
        {
            _context = context;
            _repositories = new Hashtable();

            Mentors = new MentorRepo(context,userManager,stuffRepo);
            Trainees = new TraineeRepo(context,userManager);
            Heads = new HeadRepo(context,userManager,stuffRepo);
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
