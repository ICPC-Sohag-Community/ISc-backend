using ISc.Domain.Models;

namespace ISc.Application.Interfaces.Repos
{
    public interface ITraineeRepo:IBaseRepo<Trainee>
    {
        void Delete(Trainee entity, bool isComplete);
    }
}
