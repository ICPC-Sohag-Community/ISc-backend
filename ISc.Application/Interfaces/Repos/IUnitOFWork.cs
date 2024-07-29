using ISc.Application.Dtos.Standing;
using ISc.Domain.Interface;

namespace ISc.Application.Interfaces.Repos
{
    public interface IUnitOfWork:IDisposable
    {
        IMentorRepo Mentors { get; }
        ITraineeRepo Trainees { get; }
        IHeadRepo Heads { get; }
        Task<int> SaveAsync();
        Task<StandingDto> GetStandingAsync(int campId);
        IBaseRepo<T> Repository<T>() where T : class, ISoftEntity;
    }
}
