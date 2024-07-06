using ISc.Domain.Interface;

namespace ISc.Application.Interfaces.Repos
{
    public interface IUnitOfWork:IDisposable
    {
        IMentorRepo Mentors { get; }
        ITraineeRepo Trainees { get; }
        IHeadRepo Heads { get; }
        Task<int> SaveAsync();
        IBaseRepo<T> Repository<T>() where T : class, ISoftEntity;
    }
}
