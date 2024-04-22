using ISc.Application.Interfaces.Repos;
using ISc.Domain.Models;

namespace ISc.Presistance.Repos
{
    public class TraineeRepo : BaseRepo<Trainee>, ITraineeRepo
    {
        public TraineeRepo(ICPCDbContext context) : base(context)
        {
        }
        public override void Delete(Trainee entity)
        {
            base.Delete(entity);
        }
    }
}
